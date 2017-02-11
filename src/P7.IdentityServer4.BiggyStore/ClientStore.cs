using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using P7.HugoStore.Core;
using P7.IdentityServer4.Common;

namespace P7.IdentityServer4.BiggyStore
{
    public class ClientStore : HugoStoreBase<ClientDocument>, IClientStore
    {
        public ClientStore(IIdentityServer4BiggyConfiguration biggyConfiguration) :
            base(biggyConfiguration, "client")
        {
        }

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            var cw = new ClientDocument() {Id = clientId};
            var result = await FetchAsync(cw.Id_G);
            return await result.ToClientAsync();
        }
    }
}
