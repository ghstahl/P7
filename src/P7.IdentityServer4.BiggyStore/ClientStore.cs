﻿using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using P7.Core.Linq;
using P7.HugoStore.Core;
using P7.IdentityServer4.Common;
using P7.Store;

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

        public async Task<IPage<Client>> PageAsync(int pageSize, byte[] pagingState)
        {
            byte[] currentPagingState = pagingState;
            PagingState ps = pagingState.Deserialize();
            var records = await RetrieveAsync();
            records = records.OrderBy(o => o.Id).ToList();

            var predicate = PredicateBuilder.True<ClientDocument>();

            var filtered = records.Where(predicate.Compile()).Select(i => i);

            var slice = filtered.Skip(ps.CurrentIndex).Take(pageSize).ToList();
            if (slice.Count < pageSize)
            {
                // we are at the end
                pagingState = null;
            }
            else
            {
                ps.CurrentIndex += pageSize;
                pagingState = ps.Serialize();
            }

            List<Client> clientSlice = new List<Client>();
            foreach (var item in slice)
            {
                var client = await item.MakeClientAsync();
                clientSlice.Add(client);
            }

            var page = new PageProxy<Client>(currentPagingState, pagingState, clientSlice);
            return page;
        }
    }
}
