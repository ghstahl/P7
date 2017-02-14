using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using P7.IdentityServer4.Common;
using Shouldly;

namespace Test.P7.IdentityServer4.BiggyStore
{
    [TestClass]
    public class ModelTests
    {
        private string UniqueGuid
        {
            get { return Guid.NewGuid().ToString(); }
        }

        [TestMethod]
        public void scope_model()
        {
            var scope = new Scope()
            {
                Description = UniqueGuid,
                DisplayName = UniqueGuid,
                Emphasize = true,
                Name = UniqueGuid,
                Required = true,
                ShowInDiscoveryDocument = true,
                UserClaims = new List<string>() {UniqueGuid, UniqueGuid}
            };

            var scopeModel = new ScopeModel(scope);
            var scopeModel2 = new ScopeModel(scope);

            scopeModel2.ShouldBe(scopeModel);

        }

        [TestMethod]
        public void claim_model()
        {
            var claimModel = new ClaimModel()
            {
                Type = UniqueGuid,
                Value = UniqueGuid,
                ValueType = UniqueGuid
            };
            var claimModel2 = new ClaimModel(claimModel);
            var claimModel3 = new ClaimModel()
            {
                Type = UniqueGuid,
                Value = UniqueGuid,
                ValueType = UniqueGuid
            };



            claimModel2.ShouldBe(claimModel);
            claimModel2.ShouldNotBe(claimModel3);

        }
    }
}
