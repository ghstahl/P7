using System;
using IdentityServer4.Models;
using Newtonsoft.Json;
using P7.Core.Utils;
using P7.IdentityServer4.Common;
using P7.Store;

namespace P7.IdentityServer4.BiggyStore
{
    public class ApiResourceDocument : ApiResourceModel, IDocumentBase
    {
        [JsonIgnore]
        public Guid Id_G
        {
            get
            {
                if (string.IsNullOrEmpty(Id))
                    return Guid.Empty;
                return Guid.Parse(Id);
            }
        }

        public virtual string Id { get; set; }

        public ApiResourceDocument()
        {
        }

        public ApiResourceDocument(ApiResource apiResource) : base(apiResource)
        {
            Id = GuidGenerator.CreateGuid(apiResource.Name).ToString();
        }
    }
}