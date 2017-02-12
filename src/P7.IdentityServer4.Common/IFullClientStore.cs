using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace P7.IdentityServer4.Common
{
    public interface IFullClientStore : IClientStore
    {
        Task InsertClientAsync(Client client);
        Task DeleteClientByIdAsync(string clientId);
    }
}
