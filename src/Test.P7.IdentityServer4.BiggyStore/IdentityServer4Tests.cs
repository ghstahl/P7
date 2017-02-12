using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using FakeItEasy;
using GraphQL;
using GraphQL.Http;
using IdentityServer4.Models;
using IdentityServer4.ResponseHandling;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using P7.Core;
using P7.GraphQLCore;
using P7.HugoStore.Core;
using P7.IdentityServer4.BiggyStore;
using P7.IdentityServer4.Common;
using P7.Store;
using Shouldly;

namespace Test.P7.IdentityServer4.BiggyStore
{
    class MyBiggyConfiguration : IIdentityServer4BiggyConfiguration
    {
        public string DatabaseName { get; set; }
        public string FolderStorage { get; set; }
    }

    [TestClass]
    [DeploymentItem("source", "source")]
    public class IdentityServer4Tests
    {
        public TenantDatabaseBiggyConfig GlobalTenantDatabaseBiggyConfig { get; set; }
        private string _targetFolder;
        protected string TargetFolder => _targetFolder;

        [TestInitialize]
        public void Initialize()
        {
            _targetFolder = Path.Combine(UnitTestHelpers.BaseDir, @"source",
                DateTime.Now.ToString("yyyy-dd-M__HH-mm-ss") + "_" + Guid.NewGuid().ToString());
            Directory.CreateDirectory(_targetFolder);
            GlobalTenantDatabaseBiggyConfig = new TenantDatabaseBiggyConfig();
            GlobalTenantDatabaseBiggyConfig.UsingFolder(TargetFolder);
            GlobalTenantDatabaseBiggyConfig.UsingTenantId(TenantDatabaseBiggyConfig.GlobalTenantId);
            IIdentityServer4BiggyConfiguration biggyConfiguration = new MyBiggyConfiguration()
            {
                FolderStorage = GlobalTenantDatabaseBiggyConfig.Folder,
                DatabaseName = GlobalTenantDatabaseBiggyConfig.Database
            };

            var hostName = typeof(MyAutofacFactory).GetTypeInfo().Assembly.GetName().Name;
            var hostingEnvironment = A.Fake<IHostingEnvironment>();
            var httpContextAccessor = A.Fake<IHttpContextAccessor>();


            hostingEnvironment.ApplicationName = hostName;
            Global.HostingEnvironment = hostingEnvironment;
            AutofacStoreFactory = new MyAutofacFactory() { BiggyConfiguration = biggyConfiguration };

        }

        public MyAutofacFactory AutofacStoreFactory { get; set; }

        Client MakeNewClient()
        {
            string clientId = Guid.NewGuid().ToString();
            var client = new Client()
            {
                AbsoluteRefreshTokenLifetime = 1,
                AccessTokenLifetime = 1,
                AccessTokenType = AccessTokenType.Jwt,
                AllowAccessTokensViaBrowser = true,
                AllowedCorsOrigins = new List<string>() { "a" },
                AllowedGrantTypes = new List<string>() { "a" },
                AllowedScopes = new List<string>() { "a" },
                AllowOfflineAccess = true,
                AllowPlainTextPkce = true,
                AllowRememberConsent = true,
                AlwaysIncludeUserClaimsInIdToken = true,
                AlwaysSendClientClaims = true,
                AuthorizationCodeLifetime = 1,
                ClientId = clientId,
                RequireClientSecret = true,
                RequirePkce = true,
                ProtocolType = "protocoltype",
                LogoutSessionRequired = true,
                Claims = new List<Claim>() { new Claim("a-type", "a-value") },
                ClientName = "clientName",
                ClientSecrets = new List<Secret>() { new Secret("a-value", "a-description") },
                ClientUri = "clientUri",
                EnableLocalLogin = true,
                Enabled = true,
                IdentityProviderRestrictions = new List<string>() { "a" },
                IdentityTokenLifetime = 1,
                IncludeJwtId = true,
                LogoUri = "LogoUri",
                LogoutUri = "logoutUri",
                PostLogoutRedirectUris = new List<string>() { "a" },
                PrefixClientClaims = true,
                RedirectUris = new List<string>() { "a" },
                RefreshTokenExpiration = TokenExpiration.Absolute,
                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                RequireConsent = true,
                SlidingRefreshTokenLifetime = 1,
                UpdateAccessTokenClaimsOnRefresh = true
            };
            return client;
        }

        List<Client> MakeNewClients(int count)
        {
            var result = new List<Client>();
            for (int i = 0; i < count; ++i)
            {
                result.Add(MakeNewClient());
            }
            return result;
        }
        [TestMethod]
        public async Task add_read_delete_client()
        {
            var fullClientStore = AutofacStoreFactory.Resolve<IFullClientStore>();
            var client = MakeNewClient();
            await fullClientStore.InsertClientAsync(client);
            var result = await fullClientStore.FindClientByIdAsync(client.ClientId);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ClientId == client.ClientId);

            await fullClientStore.DeleteClientByIdAsync(client.ClientId);

            result = await fullClientStore.FindClientByIdAsync(client.ClientId);
            Assert.IsNull(result);
        }
        [TestMethod]
        public async Task add_page_delete_clients()
        {
            var fullClientStore = AutofacStoreFactory.Resolve<IFullClientStore>();
            var clients = MakeNewClients(10);
            foreach (var client in clients)
            {
                await fullClientStore.InsertClientAsync(client);
                var result = await fullClientStore.FindClientByIdAsync(client.ClientId);
                Assert.IsNotNull(result);
                Assert.IsTrue(result.ClientId == client.ClientId);
            }

            var page = await fullClientStore.PageAsync(11, null);
            Assert.AreEqual(clients.Count,page.Count);
            Assert.IsNull(page.CurrentPagingState);
            Assert.IsNull(page.PagingState);

            foreach (var client in clients)
            {
                await fullClientStore.DeleteClientByIdAsync(client.ClientId);
                var result = await fullClientStore.FindClientByIdAsync(client.ClientId);
                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public async Task add_read_delete_consent()
        {
            var theStore = AutofacStoreFactory.Resolve<IUserConsentStore>();
            var consent = MakeNewConsent();

            await theStore.StoreUserConsentAsync(consent);
            var result = await theStore.GetUserConsentAsync(consent.SubjectId,consent.ClientId);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ClientId == consent.ClientId);
            Assert.IsTrue(result.SubjectId == consent.SubjectId);

            await theStore.RemoveUserConsentAsync(consent.SubjectId, consent.ClientId);

            result = await theStore.GetUserConsentAsync(consent.SubjectId, consent.ClientId);
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task add_read_delete_persisted_grant()
        {
            var theStore = AutofacStoreFactory.Resolve<IPersistedGrantStore>();
            var grant = MakeNewPersistedGrant();

            await theStore.StoreAsync(grant);

            var result = await theStore.GetAsync(grant.Key);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ClientId == grant.ClientId);
            Assert.IsTrue(result.SubjectId == grant.SubjectId);
            Assert.IsTrue(result.Type == grant.Type);

            await theStore.RemoveAsync(grant.Key);

            result = await theStore.GetAsync(grant.Key);
            Assert.IsNull(result);
        }
        [TestMethod]
        public async Task add_read_delete_persisted_grants()
        {
            var theStore = AutofacStoreFactory.Resolve<IPersistedGrantStore>();
            var grants = MakeNewPersistedGrants(10);

            var clientId = Guid.NewGuid().ToString();
            var subjectId = Guid.NewGuid().ToString();
            var type = Guid.NewGuid().ToString();
            int i = 0;
            foreach (var grant in grants)
            {
                grant.ClientId = clientId;
                if (i % 2 == 0)
                {
                    grant.SubjectId = subjectId;
                    grant.Type = type;
                }
                await theStore.StoreAsync(grant);
                ++i;
            }

            var result = await theStore.GetAllAsync(subjectId);

            Assert.IsNotNull(result);
            Assert.AreEqual(grants.Count/2, result.Count());


            await theStore.RemoveAllAsync(subjectId,clientId);
            result = await theStore.GetAllAsync(subjectId);
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public async Task add_read_delete_persisted_grants_and_types()
        {
            var theStore = AutofacStoreFactory.Resolve<IPersistedGrantStore>();
            var grants = MakeNewPersistedGrants(10);

            var clientId = Guid.NewGuid().ToString();
            var subjectId = Guid.NewGuid().ToString();

            foreach (var grant in grants)
            {
                grant.ClientId = clientId;
                grant.SubjectId = subjectId;
                await theStore.StoreAsync(grant);
            }



            var grantR = grants[0];
            await theStore.RemoveAllAsync(grantR.SubjectId, grantR.ClientId, grantR.Type);
            var result = await theStore.GetAllAsync(subjectId);

            Assert.IsNotNull(result);
            Assert.AreEqual(grants.Count -1, result.Count());
        }
        private List<PersistedGrant> MakeNewPersistedGrants(int count)
        {
            var final = new List<PersistedGrant>();

            for (int i = 0; i < count; ++i)
            {
                final.Add(MakeNewPersistedGrant());
            }
            return final;
        }
        private PersistedGrant MakeNewPersistedGrant()
        {
            return new PersistedGrant()
            {
             ClientId = Guid.NewGuid().ToString(),
             CreationTime = DateTime.UtcNow,
             Data = Guid.NewGuid().ToString(),
             Expiration = DateTime.UtcNow,
             Key = Guid.NewGuid().ToString(),
             SubjectId = Guid.NewGuid().ToString(),
             Type = Guid.NewGuid().ToString()
            };
        }

        private Consent MakeNewConsent()
        {
            return new Consent()
            {
                ClientId = Guid.NewGuid().ToString(),
                CreationTime = DateTime.UtcNow,
                Expiration = DateTime.UtcNow,
                Scopes = new List<string>() {"a-scope"},
                SubjectId = Guid.NewGuid().ToString()
            };
        }

        [TestMethod]
        public void paging_state_conversions()
        {
            PagingState pagingStateExpected = new PagingState() { CurrentIndex = 1234 };
            var bytes = pagingStateExpected.Serialize();
            var pagingState = bytes.Deserialize();

            pagingState.ShouldBe(pagingStateExpected);

            var psString = Convert.ToBase64String(bytes);
            bytes = Convert.FromBase64String(psString);
            pagingState = bytes.Deserialize();

            pagingState.ShouldBe(pagingStateExpected);

            var urlEncodedPagingState = WebUtility.UrlEncode(psString);
            var psStringUrlDecoded = WebUtility.UrlDecode(urlEncodedPagingState);

            psStringUrlDecoded.ShouldBe(psString);
            bytes = Convert.FromBase64String(psStringUrlDecoded);
            pagingState = bytes.Deserialize();
            pagingState.ShouldBe(pagingStateExpected);
        }
    }
}
