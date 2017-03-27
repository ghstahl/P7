using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace P7.Core.Identity
{
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
            ((ClaimsIdentity)context.Principal.Identity).AddClaims(await _postAuthClaimsProvider.FetchClaims(context));
            return context.Principal;
        }
    }
}