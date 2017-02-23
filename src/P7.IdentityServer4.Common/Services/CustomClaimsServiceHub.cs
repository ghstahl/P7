using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.Extensions.Logging;

namespace P7.IdentityServer4.Common.Services
{
    public interface ICustomClaimsService : IClaimsService
    {
        string Name { get; }
    }
    public class CustomArbitraryClaimsService : DefaultClaimsService, ICustomClaimsService
    {
        public CustomArbitraryClaimsService(IProfileService profile, ILogger<DefaultClaimsService> logger) : base(profile, logger)
        {
        }

        public string Name => "arbitrary-claims";
    }
    public class CustomOpenIdClaimsService : DefaultClaimsService, ICustomClaimsService
    {
        public CustomOpenIdClaimsService(IProfileService profile, ILogger<DefaultClaimsService> logger) : base(profile, logger)
        {
        }
        public string Name => "arbitrary-openid-claims";
    }
    public class CustomClaimsServiceHub : DefaultClaimsService
    {
        private List<ICustomClaimsService> _customClaimsServices;
        public CustomClaimsServiceHub(IProfileService profile, ILogger<DefaultClaimsService> logger,
            IEnumerable<ICustomClaimsService> customClaimsServices ) : base(profile, logger)
        {
            _customClaimsServices = customClaimsServices.ToList();
        }

        public override Task<IEnumerable<Claim>> GetAccessTokenClaimsAsync(ClaimsPrincipal subject, Client client, Resources resources, ValidatedRequest request)
        {
            var handler = request.Raw["handler"];
            if (string.IsNullOrEmpty(handler))
            {
                return base.GetAccessTokenClaimsAsync(subject, client, resources, request);
            }
            var query = from item in _customClaimsServices
                where string.Compare(item.Name, handler, StringComparison.OrdinalIgnoreCase) == 0
                select item;
            var customClaimsService = query.FirstOrDefault();
            if (customClaimsService == null)
            {
                return base.GetAccessTokenClaimsAsync(subject, client, resources, request);
            }
            else
            {
                return customClaimsService.GetAccessTokenClaimsAsync(subject, client, resources, request);
            }
        }
    }
}
