using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace P7.IdentityServer4.BiggyStore
{
    public class ClientStore : IClientStore
    {
        public ClientStore()
        {
        }

        public Task<Client> FindClientByIdAsync(string clientId)
        {
            throw new NotImplementedException();
        }
    }
}
