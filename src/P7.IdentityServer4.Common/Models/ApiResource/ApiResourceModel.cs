using System;
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

        public override bool Equals(object obj)
        {
            var other = obj as ApiResourceModel;
            if (other == null)
            {
                return false;
            }
            var differenceUserClaims = UserClaims.Except(other.UserClaims);
            var equalsUserClaims = !differenceUserClaims.Any();

            var differencesApiSecrets = ApiSecrets.Except(other.ApiSecrets);
            var equalsUserApiSecrets = !differencesApiSecrets.Any();

            var differencesScopes = Scopes.Except(other.Scopes);
            var equalsScopes = !differencesScopes.Any();

            var result = equalsUserApiSecrets
                         && Description.Equals(other.Description)
                         && DisplayName.Equals(other.DisplayName)
                         && Enabled.Equals(other.Enabled)
                         && Name.Equals(other.Name)
                         && equalsScopes
                         && Description.Equals(other.Description)
                         && equalsUserClaims;
            return result;
        }

        public override int GetHashCode()
        {
            var code = Name.GetHashCode();
            return code;
        }
    }
}

