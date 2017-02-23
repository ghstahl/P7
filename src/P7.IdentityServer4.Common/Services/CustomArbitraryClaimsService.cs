using IdentityServer4.Services;
using Microsoft.Extensions.Logging;

namespace P7.IdentityServer4.Common.Services
{
    public class CustomArbitraryClaimsService : DefaultClaimsService, ICustomClaimsService
    {
        public CustomArbitraryClaimsService(IProfileService profile, ILogger<DefaultClaimsService> logger) : base(profile, logger)
        {
        }

        public string Name => "arbitrary-claims";
    }
}