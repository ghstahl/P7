using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace P7.SimpleRedirector.Area.Controllers
{
    [Area("CDNInternal")]
    public class HomeController : Controller
    {
        private ILogger Logger { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession Session => _httpContextAccessor.HttpContext.Session;

        public HomeController(IHttpContextAccessor httpContextAccessor,
            ILogger<HomeController> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            Logger = logger;
        }

        public async Task<ActionResult> Index(string key, string remaining)
        {
            /*
            var configRoot = await _configuration.GetConfigurationAsync("SimpleRedirect.Configuration");
            var redirects = configRoot.Redirects;


            var resultRecord = redirects
                .Where(record => record.Key == id)
                .Select(record => record)
                .FirstOrDefault();

            if (resultRecord != null)
            {
                var url = Request.Url;
                string scheme = (resultRecord.Scheme) ?? url.Scheme;

                var realUrl = string.Format("{0}://{1}/{2}", scheme, resultRecord.BaseUrl, remaining);
                return new RedirectResult(realUrl);
            }
            throw new HttpException((int)HttpStatusCode.NotFound, "NotFound");*/
            return Redirect("/");
        }
    }
}
