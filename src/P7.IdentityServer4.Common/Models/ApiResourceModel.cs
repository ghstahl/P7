using System.Collections.Generic;
using IdentityServer4.Models;

namespace P7.IdentityServer4.Common
{
    public class ApiResourceModel
    {
        public List<Secret> ApiSecrets { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public List<ScopeModel> Scopes { get; set; }
        public List<string> UserClaims { get; set; }
    }
}