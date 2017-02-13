using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;

namespace P7.IdentityServer4.Common
{
    public class ApiResourceModel :
        AbstractApiResourceModel<List<Secret>, List<ScopeModel>, List<string>>
    {
        public ApiResourceModel() { }
        public ApiResourceModel(ApiResource apiResource) : base(apiResource)
        {
        }

        internal override List<string> Serialize(ICollection<string> userClaims)
        {
            return userClaims.ToList();
        }

        internal override List<ScopeModel> Serialize(ICollection<Scope> scopes)
        {
            return scopes.ToScopeModels();
        }

        public override List<Secret> Serialize(ICollection<Secret> apiSecrets)
        {
            return apiSecrets.ToList();
        }

        protected override async Task<List<string>> DeserializeUserClaimsAsync(List<string> userClaims)
        {
            return userClaims;
        }

        protected override async Task<List<ScopeModel>> DeserializeScopesAsync(List<ScopeModel> scopes)
        {
            return scopes;
        }

        protected override async Task<List<Secret>> DeserializeApiSecretsAsync(List<Secret> apiSecrets)
        {
            return apiSecrets;
        }
    }
}