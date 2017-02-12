using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using P7.HugoStore.Core;
using P7.IdentityServer4.Common;

namespace P7.IdentityServer4.BiggyStore
{
    public class ClientStore : HugoStoreBase<ClientDocument>, IFullClientStore
    {
        public ClientStore(IIdentityServer4BiggyConfiguration biggyConfiguration) :
            base(biggyConfiguration, "client")
        {
        }

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            var doc = new ClientDocument(new Client() {ClientId = clientId});
            var result = await FetchAsync(doc.Id_G);
            if (result == null)
                return null;
            return await result.ToClientAsync();
        }

        public async Task InsertClientAsync(Client client)
        {
            var doc = new ClientDocument(client);
            await InsertAsync(doc);
        }

        public async Task DeleteClientByIdAsync(string clientId)
        {
            var doc = new ClientDocument(new Client() { ClientId = clientId });
            await DeleteAsync(doc.Id_G);
        }
    }
}
