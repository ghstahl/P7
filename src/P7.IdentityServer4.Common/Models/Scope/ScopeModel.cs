using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;

namespace P7.IdentityServer4.Common
{
    public class ScopeModel :
        AbstractScopeModel<List<string>>
    {
        public ScopeModel()
        {
        }

        public ScopeModel(Scope scope) : base(scope)
        {
        }

        internal override List<string> Serialize(ICollection<string> userClaims)
        {
            return userClaims.ToList();
        }

        protected override async Task<List<string>> DeserializeUserClaimsAsync(List<string> obj)
        {
            return obj;
        }
    }
}