﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using P7.Core.Utils;
using P7.Store;

namespace P7.RazorProvider.Store.Hugo.Models
{
    public class RazorLocationDocument : RazorLocation, IDocumentBaseWithTenant
    {
        public RazorLocationDocument() { }
        public RazorLocationDocument(RazorLocation model) : base(model)
        {
            Id = GuidGenerator.CreateGuid(model.Location).ToString();
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
