using System.Security.Claims;

namespace P7.IdentityServer4.Common
{
    public class ClaimModel
    {
        public ClaimModel()
        {
        }

        public ClaimModel(Claim claim)
        {
            Type = claim.Type;
            Value = claim.Value;
            ValueType = claim.ValueType;
        }

        public string Type { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }
    }
}