﻿using System;
using IdentityServer4.Models;
using Newtonsoft.Json;
using P7.Core.Utils;
using P7.IdentityServer4.Common;
using P7.Store;

namespace P7.IdentityServer4.BiggyStore
{
   
    public class ClientDocument : ClientModel, IDocumentBaseWithTenant
    {
        public ClientDocument() { }
        public ClientDocument(Client client):base(client)
        {
            Id = GuidGenerator.CreateGuid(client.ClientId).ToString();
        }

        [JsonIgnore]
        public Guid TenantId_G
        {
            get
            {
                if (string.IsNullOrEmpty(TenantId))
                    return Guid.Empty;

                return Guid.Parse(TenantId);
            }
        }
        public virtual string TenantId { get; set; }
       
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
    }
}