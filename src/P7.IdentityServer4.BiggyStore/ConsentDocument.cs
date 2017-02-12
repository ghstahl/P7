﻿using System;
using IdentityServer4.Models;
using Newtonsoft.Json;
using P7.BlogStore.Core;
using P7.Core.Utils;
using P7.IdentityServer4.Common.Models.Consent;

namespace P7.IdentityServer4.BiggyStore
{
    public class ConsentDocument : ConsentModel, IDocumentBase
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

        public ConsentDocument() { }
        public ConsentDocument(Consent client) : base(client)
        {
            Id = GuidGenerator.CreateGuid(client.ClientId, client.SubjectId).ToString();
        }
    }
}