using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;

namespace P7.IdentityServer4.Common.Services
{
    public class CustomClaimsServiceHub : DefaultClaimsService
    {
        public CustomClaimsServiceHub(IProfileService profile, ILogger<DefaultClaimsService> logger) : base(profile, logger)
        {
        }
    }
}
