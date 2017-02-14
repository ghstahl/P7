using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;

namespace P7.IdentityServer4.Common
{
    public class IdentityResourceModel :
        AbstractIdentityResourceModel<List<string>>
    {
        public IdentityResourceModel()
        {
        }

        public IdentityResourceModel(IdentityResource identityResource) : base(identityResource)
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
            var other = obj as IdentityResourceModel;
            if (other == null)
            {
                return false;
            }
            var differenceUserClaims = UserClaims.Except(other.UserClaims);
            var equalsUserClaims = !differenceUserClaims.Any();



            var result = equalsUserClaims
                         && Description.Equals(other.Description)
                         && DisplayName.Equals(other.DisplayName)
                         && Enabled.Equals(other.Enabled)
                         && Emphasize.Equals(other.Emphasize)
                         && Required.Equals(other.Required)
                         && ShowInDiscoveryDocument.Equals(other.ShowInDiscoveryDocument);
            return result;
        }

        public override int GetHashCode()
        {
            var code = Name.GetHashCode();
            return code;
        }
    }
}
