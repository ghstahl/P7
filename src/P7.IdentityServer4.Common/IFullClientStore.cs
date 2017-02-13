using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using P7.Store;

namespace P7.IdentityServer4.Common
{
    public interface IFullResourceStore:IResourceStore, IIdentityResourceStore, IApiResourceStore
    {
        
    }
    public interface IIdentityResourceStore
    {
        Task InsertIdentityResourceAsync(IdentityResource identityResource);

        Task DeleteIdentityResourceByNameAsync(string name);

        Task<IPage<IdentityResource>> PageAsync(int pageSize,
            byte[] pagingState);
    }

    public interface IApiResourceStore
    {
        Task InsertApiResourceAsync(ApiResource apiResource);

        Task DeleteApiResourceByNameAsync(string name);

        Task<IPage<ApiResource>> PageAsync(int pageSize,
            byte[] pagingState);
    }

    public interface IFullClientStore : IClientStore
    {
        /// <summary>
        /// Inserts or Updates a new Client
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        Task InsertClientAsync(Client client);

        /// <summary>
        /// Deletes a client
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Task DeleteClientByIdAsync(string clientId);

        /// <summary>
        /// Pages all documents
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pagingState"></param>
        /// <returns></returns>
        Task<IPage<Client>> PageAsync(int pageSize,
            byte[] pagingState);
    }
}
