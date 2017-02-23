using IdentityServer4.Services;
using Microsoft.Extensions.Logging;

namespace P7.IdentityServer4.Common.Services
{
    public class CustomOpenIdClaimsService : DefaultClaimsService, ICustomClaimsService
    {
        public CustomOpenIdClaimsService(IProfileService profile, ILogger<DefaultClaimsService> logger) : base(profile, logger)
        {
        }
        public string Name => "arbitrary-openid-claims";
    }
}