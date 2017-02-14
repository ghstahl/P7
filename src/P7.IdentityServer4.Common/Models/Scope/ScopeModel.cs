using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

        public override bool Equals(object obj)
        {
            var other = obj as ScopeModel;
            if (other == null)
            {
                return false;
            }

            IEnumerable<string> difference = UserClaims.Except(other.UserClaims);
            var equalsUserClaims = !difference.Any();
             
            var result = Description.Equals(other.Description)
                   && DisplayName.Equals(other.DisplayName)
                   && Emphasize.Equals(other.Emphasize)
                   && Name.Equals(other.Name)
                   && Required.Equals(other.Required)
                   && ShowInDiscoveryDocument.Equals(other.ShowInDiscoveryDocument)
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