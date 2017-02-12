﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using FakeItEasy;
using GraphQL;
using GraphQL.Http;
using IdentityServer4.Models;
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

        [TestMethod]
        public async Task add_read_delete_client()
        {
            var fullClientStore = AutofacStoreFactory.Resolve<IFullClientStore>();
            string clientId = Guid.NewGuid().ToString();
            var client = new Client()
            {
                AbsoluteRefreshTokenLifetime = 1,
                AccessTokenLifetime = 1,
                AccessTokenType = AccessTokenType.Jwt,
                AllowAccessTokensViaBrowser = true,
                AllowedCorsOrigins = new List<string>() {"a"},
                AllowedGrantTypes = new List<string>() {"a"},
                AllowedScopes = new List<string>() {"a"},
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
                Claims = new List<Claim>() {new Claim("a-type", "a-value")},
                ClientName = "clientName",
                ClientSecrets = new List<Secret>() {new Secret("a-value", "a-description")},
                ClientUri = "clientUri",
                EnableLocalLogin = true,
                Enabled = true,
                IdentityProviderRestrictions = new List<string>() {"a"},
                IdentityTokenLifetime = 1,
                IncludeJwtId = true,
                LogoUri = "LogoUri",
                LogoutUri = "logoutUri",
                PostLogoutRedirectUris = new List<string>() {"a"},
                PrefixClientClaims = true,
                RedirectUris = new List<string>() {"a"},
                RefreshTokenExpiration = TokenExpiration.Absolute,
                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                RequireConsent = true,
                SlidingRefreshTokenLifetime = 1,
                UpdateAccessTokenClaimsOnRefresh = true
            };
            await fullClientStore.InsertClientAsync(client);
            var result = await fullClientStore.FindClientByIdAsync(client.ClientId);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ClientId == clientId);

            await fullClientStore.DeleteClientByIdAsync(client.ClientId);

            result = await fullClientStore.FindClientByIdAsync(client.ClientId);
            Assert.IsNull(result);
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
