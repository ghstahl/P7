using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Endpoints;
using IdentityServer4.Hosting;
using IdentityServer4.ResponseHandling;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace P7.IdentityServer4.Common.Endpoints
{
    public abstract class CustomTokenEndpoint : TokenEndpoint, IEndpoint
    {
        public CustomTokenEndpoint(ITokenRequestValidator requestValidator, ClientSecretValidator clientValidator, ITokenResponseGenerator responseGenerator, ILogger<TokenEndpoint> logger) : base(requestValidator, clientValidator, responseGenerator, logger)
        {
        }

        public new async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            return await base.ProcessAsync(context);
        }
    }
}
