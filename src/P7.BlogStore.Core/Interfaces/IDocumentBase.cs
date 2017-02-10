using System;
using Newtonsoft.Json;

namespace P7.BlogStore.Core
{
    public class DocumentBase : IDocumentBase
    {
        [JsonIgnore]
        public Guid Id_G
        {
            get
            {
                if(string.IsNullOrEmpty(Id))
                    return Guid.Empty;

                return Guid.Parse(Id);
            }
        }

        public virtual string Id { get; set; }
    }

    public interface IDocumentBase
    {
        Guid Id_G { get;  }
        string Id { get; }
    }
}