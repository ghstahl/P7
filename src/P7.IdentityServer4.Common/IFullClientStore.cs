using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;

namespace P7.IdentityServer4.Common
{
    public interface IFullClientStore
    {
        Task InsertClientAsync(Client client);
        Task DeleteClientByIdAsync(string clientId);
    }
}
