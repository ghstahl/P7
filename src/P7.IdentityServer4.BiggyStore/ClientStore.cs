using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Newtonsoft.Json;
using P7.BlogStore.Core;
using P7.HugoStore.Core;

namespace P7.IdentityServer4.BiggyStore
{
    public interface IIdentityServer4BiggyConfiguration: IBiggyConfiguration
    {
        
    }

    public class ClientWrapper : Client, IDocumentBase
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
    }

    public class ClientStore : HugoStoreBase<ClientWrapper>, IClientStore
    {
        public ClientStore(IIdentityServer4BiggyConfiguration biggyConfiguration) :
            base(biggyConfiguration,"client")
        {

        }
        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            var cw = new ClientWrapper() {Id = clientId};
            var result = await FetchAsync(cw.Id_G);
            return result;
        }
    }
}
