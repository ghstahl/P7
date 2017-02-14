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

        public override bool Equals(object obj)
        {
            var other = obj as PersistedGrantModel;
            if (other == null)
            {
                return false;
            }


            var result = ClientId.Equals(other.ClientId)
                   && CreationTime.Equals(other.CreationTime)
                   && Data.Equals(other.Data)
                   && Expiration.Equals(other.Expiration)
                   && Key.Equals(other.Key)
                   && SubjectId.Equals(other.SubjectId)
                   && Type.Equals(other.Type);
            return result;
        }

        public override int GetHashCode()
        {
            var code = ClientId.GetHashCode() ^
                   CreationTime.GetHashCode() ^
                   Data.GetHashCode() ^
                   Expiration.GetHashCode() ^
                   Key.GetHashCode() ^
                   SubjectId.GetHashCode() ^
                   Type.GetHashCode();
            return code;
        }
    }
}

 