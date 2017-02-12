namespace P7.IdentityServer4.Common
{
    public class PersistedGrantModel : AbstractPersistedGrantModel
    {
        public PersistedGrantModel()
        {
        }
        public PersistedGrantModel(global::IdentityServer4.Models.PersistedGrant grant):base(grant)
        {
        }
    }
}