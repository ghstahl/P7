using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace P7.Core.Identity
{
    public interface IPostAuthClaimsProvider
    {
        Task<List<Claim>> FetchClaims(ClaimsTransformationContext context);
    }
}