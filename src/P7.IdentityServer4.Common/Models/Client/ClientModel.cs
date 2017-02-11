using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;

namespace P7.IdentityServer4.Common
{
    public class ClientModel :
        AbstractClientModel<
            List<ClaimModel>,
            List<Secret>,
            List<string>
        >
    {
        public ClientModel()
            : base()
        {
        }

        public ClientModel(Client client) : base(client)
        {
        }

        public override List<string> Serialize(List<string> stringList)
        {
            return stringList;
        }

        public override async Task<List<Claim>> DeserializeClaimsAsync(List<ClaimModel> obj)
        {
            return await Task.FromResult(obj == null ? null : obj.ToClaims());
        }

        public override List<ClaimModel> Serialize(List<Claim> claims)
        {
            return claims == null ? null : claims.ToClaimTypeRecords();
        }

        public override List<Secret> Serialize(List<Secret> secrets)
        {
            return secrets;
        }

        public override async Task<List<string>> DeserializeStringsAsync(List<string> obj)
        {
            return await Task.FromResult(obj);
        }

        public override async Task<List<Secret>> DeserializeSecretsAsync(List<Secret> obj)
        {
            return await Task.FromResult(obj);
        }
    }
}