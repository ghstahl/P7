using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;


namespace P7.IdentityServer4.Common
{
    public static class PersistedGrantExtensions
    {
        public static global::IdentityServer4.Models.PersistedGrant ToPersistedGrant(this PersistedGrantModel model)
        {
            var result = model.ToPersistedGrantAsync();
            return result.Result;
        }

        public static async Task<global::IdentityServer4.Models.PersistedGrant> ToPersistedGrantAsync(
            this PersistedGrantModel model)
        {
            var result = await model.MakePersistedGrantAsync();
            return result;
        }

        public static PersistedGrantModel ToPersistedGrantModel(this global::IdentityServer4.Models.PersistedGrant model)
        {
            return new PersistedGrantModel(model);
        }
    }
}
