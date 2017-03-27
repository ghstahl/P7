using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace P7.Core.Identity
{
    /*
     * This transform will react to any authorization, including cookies and bearer tokens.
     * We are exluding adding anything to bear tokens.
     * 
     * I still have a problem here:  
     * TODO: Find how to interigate the claims and if it a bearer token, and it comes in with a claim we are writing, 
     * then we are being frauded.
     * 
     * We want to add only claims to local cookie only stuff, and in this case "client_id":local so that this passes an allowed claims api check downstream.
     * */
    public class ClaimsTransformer : IPostAuthClaimsTransformer
    {
        private readonly ILogger _logger;
        public readonly IPostAuthClaimsProvider _postAuthClaimsProvider;
        public ClaimsTransformer(ILoggerFactory loggerFactory, IPostAuthClaimsProvider postAuthClaimsProvider)
        {
            _logger = loggerFactory.CreateLogger<ClaimsTransformer>();
            _postAuthClaimsProvider = postAuthClaimsProvider;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsTransformationContext context)
        {
            var authorizationList = context.Context.Request.Headers.Where(a => string.Compare(a.Key, "Authorization",StringComparison.CurrentCultureIgnoreCase)==0);
            if (!authorizationList.Any())
            {
                var identity = ((ClaimsIdentity)context.Principal.Identity);
                identity.AddClaims(await _postAuthClaimsProvider.FetchClaims(context));
            }
            return context.Principal;
        }
    }
}