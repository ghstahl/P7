using System.Collections.Generic;
using System.Linq;
using IdentityServer4.Models;

namespace P7.IdentityServer4.Common
{
    public static class ScopeExtensions
    {
        public static Scope ToScope(this ScopeModel model)
        {
            return new Scope()
            {
                Description = model.Description,
                DisplayName = model.DisplayName,
                Emphasize = model.Emphasize,
                Name = model.Name,
                Required = model.Required,
                ShowInDiscoveryDocument = model.ShowInDiscoveryDocument,
                UserClaims = model.UserClaims
            };
        }

        public static List<Scope> ToScopes(this List<ScopeModel> models)
        {
            var query = from model in models
                let c = model.ToScope()
                select c;
            return query.ToList();
        }

        public static ScopeModel ToScopeModel(this Scope model)
        {
            return new ScopeModel()
            {
                Description = model.Description,
                DisplayName = model.DisplayName,
                Emphasize = model.Emphasize,
                Name = model.Name,
                Required = model.Required,
                ShowInDiscoveryDocument = model.ShowInDiscoveryDocument,
                UserClaims = model.UserClaims.ToList()
            };
        }
        public static List<ScopeModel> ToScopeModels(this ICollection<Scope> models)
        {
            var query = from model in models
                let c = model.ToScopeModel()
                select c;
            return query.ToList();

        }
    }
}