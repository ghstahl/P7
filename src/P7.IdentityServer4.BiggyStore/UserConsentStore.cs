using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using P7.HugoStore.Core;
using P7.IdentityServer4.Common;

namespace P7.IdentityServer4.BiggyStore
{
    public class UserConsentStore : HugoStoreBase<ConsentDocument>, IUserConsentStore
    {
        public UserConsentStore(IIdentityServer4BiggyConfiguration biggyConfiguration) :
            base(biggyConfiguration, "consent")
        {
        }

        public async Task StoreUserConsentAsync(Consent consent)
        {
            var doc = new ConsentDocument(consent);
            await InsertAsync(doc);
        }

        public async Task<Consent> GetUserConsentAsync(string subjectId, string clientId)
        {
            var consent =  new Consent()
            {
                ClientId = clientId,
                SubjectId = subjectId
            };
            var doc = new ConsentDocument(consent);
            var result = await FetchAsync(doc.Id_G);
            if (result == null)
                return null;
            return await result.ToConsentAsync();
        }

        public async Task RemoveUserConsentAsync(string subjectId, string clientId)
        {
            var doc = new ConsentDocument(new Consent() { ClientId = clientId, SubjectId = subjectId });
            await DeleteAsync(doc.Id_G);
        }
    }
}