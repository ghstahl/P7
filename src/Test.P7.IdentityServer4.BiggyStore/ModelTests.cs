using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using IdentityServer4.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using P7.IdentityServer4.Common;
using Shouldly;

namespace Test.P7.IdentityServer4.BiggyStore
{
    [TestClass]
    public class ModelTests
    {
        public static string UniqueGuid
        {
            get { return Guid.NewGuid().ToString(); }
        }

        public static ScopeModel NewScopeModel
        {
            get
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
                return scopeModel;
            }
        }

        public static Secret NewSecret
        {
            get
            {
                var model = new Secret()
                {
                    Description = UniqueGuid,
                    Expiration = DateTime.UtcNow,
                    Type = UniqueGuid,
                    Value = UniqueGuid
                };
                return model;
            }
        }

        public static ApiResourceModel NewApiResourceModel
        {
            get
            {
                var model = new ApiResourceModel()
                {
                    DisplayName = UniqueGuid,
                    ApiSecrets = new EditableList<Secret>()
                    {
                        NewSecret,
                        NewSecret
                    },
                    Description = UniqueGuid,
                    Enabled = true,
                    Name = UniqueGuid,
                    Scopes = new List<ScopeModel>()
                    {
                        NewScopeModel,
                        NewScopeModel
                    },
                    UserClaims = new List<string>() {UniqueGuid, UniqueGuid}
                };
                return model;
            }
        }

        public static IdentityResourceModel NewIdentityResourceModel
        {
            get
            {
                var model = new IdentityResourceModel()
                {
                    DisplayName = UniqueGuid,
                    Description = UniqueGuid,
                    Enabled = true,
                    Name = UniqueGuid,
                    UserClaims = new List<string>() {UniqueGuid, UniqueGuid},
                    Emphasize = true,
                    Required = true,
                    ShowInDiscoveryDocument = true
                };
                return model;
            }
        }

        public static PersistedGrant NewPersistedGrant
        {
            get
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
        }

        public static List<PersistedGrant> MakeNewPersistedGrants(int count)
        {
            var final = new List<PersistedGrant>();

            for (int i = 0; i < count; ++i)
            {
                final.Add(NewPersistedGrant);
            }
            return final;
        }

        [TestMethod]
        public void scope_model()
        {

            var scopeModel = NewScopeModel;
            var scopeModel2 = new ScopeModel(scopeModel.ToScope());
            var scopeModel3 = NewScopeModel;

            scopeModel2.ShouldBe(scopeModel);
            scopeModel3.ShouldNotBe(scopeModel);
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
        [TestMethod]
        public void api_resource_model()
        {
            var model = NewApiResourceModel;
            var model2 = new ApiResourceModel(model.ToApiResource());

            var model3 = NewApiResourceModel;

            model.ShouldBe(model2);
            model.ShouldNotBe(model3);
        }
        [TestMethod]
        public void identity_resource_model()
        {
            var model = NewIdentityResourceModel;
            var model2 = new IdentityResourceModel(model.ToIdentityResource());

            var model3 = NewIdentityResourceModel;

            model.ShouldBe(model2);
            model.ShouldNotBe(model3);
        }
    }
}
