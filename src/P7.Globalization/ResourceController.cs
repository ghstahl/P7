using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using P7.Core.Localization;
using P7.Core.Reflection;

namespace P7.Globalization
{
    public static class ResourceApiExtensions
    {
        public static int GetSequenceHashCode<T>(this IEnumerable<T> sequence)
        {
            return sequence
                .Select(item => item.GetHashCode())
                .Aggregate((total, nextCode) => total ^ nextCode);
        }
    }

    [Route("ResourceApi/[controller]")]
    public class ResourceController : Controller
    {
        private ILogger Logger { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession Session => _httpContextAccessor.HttpContext.Session;
        private IStringLocalizerFactory _localizerFactory;
        private IRequestCultureProvider _requestCultureProvider;
        private readonly IStringLocalizer<ResourceController> _localizer;
        private IMemoryCache _cache;


        public ResourceController(IHttpContextAccessor httpContextAccessor,
            IMemoryCache memoryCache,
            IStringLocalizerFactory localizerFactory,
            ILogger<ResourceController> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _cache = memoryCache;
            _localizerFactory = localizerFactory;
    //        _requestCultureProvider = requestCultureProvider;
            Logger = logger;
        }

        [Route("ByDynamic")]
        [Produces(typeof(object))]
        public async Task<IActionResult> GetResourceSet(string id, string treatment)
        {
            var rqf = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            // Culture contains the information of the requested culture
            var currentCulture = rqf.RequestCulture.Culture;

            // Load Header collection into NameValueCollection object.
            var headers = _httpContextAccessor.HttpContext.Request.Headers;
            if (headers.ContainsKey("X-Culture"))
            {
                var hCulture = headers["X-Culture"];
                CultureInfo hCultureInfo = currentCulture;
                try
                {
                    hCultureInfo = new CultureInfo(hCulture);
                }
                catch (Exception)
                {
                    hCultureInfo = currentCulture;
                }
                currentCulture = hCultureInfo;
            }
            var key = new List<object> { currentCulture, id, treatment }.AsReadOnly().GetSequenceHashCode();
            var newValue = new Lazy<object>(() => { return InternalGetResourceSet(id, treatment, currentCulture); });


            var value = _cache.GetOrCreate(key.ToString(CultureInfo.InvariantCulture), entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(100);
                return newValue;
            });

            var result = value != null ? value.Value : newValue.Value;
            return Ok(result);
        }


        private object InternalGetResourceSet(string id, string treatment, CultureInfo cultureInfo)
        {
            try
            {
                var typeId = TypeHelper<Type>.GetTypeByFullName(id);
                if (typeId != null)
                {
                    if (string.IsNullOrEmpty(treatment))
                    {
                        treatment = "P7.Core.Localization.Treatment,P7.Core";
                    }
                    var typeTreatment = TypeHelper<Type>.GetTypeByFullName(treatment);
                    var localizer = _localizerFactory.Create(typeId);

                    var resourceSet = localizer.WithCulture(cultureInfo).GetAllStrings(true);
                    var instance = Activator.CreateInstance(typeTreatment) as ILocalizedStringResultTreatment;
                    var result = instance.Process(resourceSet);
                    return result;
                }
            }
            catch (Exception e)
            {
                return "";
            }
            return "";
        }
    }
}
