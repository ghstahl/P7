using System.Collections.Generic;
using System.Linq;
using IdentityServer4.Models;

namespace P7.IdentityServer4.Common
{
    public static class ApiResourceExtensions
    {
        public static ApiResource ToApiResource(this ApiResourceModel model)
        {
            return new ApiResource()
            {
                ApiSecrets = model.ApiSecrets,
                Description = model.Description,
                DisplayName = model.DisplayName,
                Enabled = model.Enabled,
                Name = model.Name,
                Scopes = model.Scopes.ToScopes(),
                UserClaims = model.UserClaims
            };
        }
        public static List<ApiResource> ToApiResources(this List<ApiResourceModel> models)
        {
            var query = from model in models
                let c = model.ToApiResource()
                select c;
            return query.ToList();
        }

        public static ApiResourceModel ToApiResourceModel(this ApiResource model)
        {
            return new ApiResourceModel()
            {
                ApiSecrets = model.ApiSecrets.ToList(),
                Description = model.Description,
                DisplayName = model.DisplayName,
                Enabled = model.Enabled,
                Name = model.Name,
                Scopes = model.Scopes.ToScopeModels(),
                UserClaims = model.UserClaims.ToList()
            };
        }
        public static List<ApiResourceModel> ToApiResourceModels(this ICollection<ApiResource> models)
        {
            var query = from model in models
                let c = model.ToApiResourceModel()
                select c;
            return query.ToList();
        }
    }
}