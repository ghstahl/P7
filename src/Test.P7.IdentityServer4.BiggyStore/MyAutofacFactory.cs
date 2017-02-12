using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using Autofac;
using FakeItEasy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using P7.Core;
using P7.GraphQLCore;
using P7.GraphQLCore.Validators;
using P7.HugoStore.Core;
using P7.IdentityServer4.BiggyStore;
using P7.IdentityServer4.Common;

namespace Test.P7.IdentityServer4.BiggyStore
{
    public class MyAutofacFactory
    {
        public IIdentityServer4BiggyConfiguration BiggyConfiguration { get; set; }

        private IContainer _autofacContainer;

        public IContainer AutofacContainer
        {
            get
            {
                if (_autofacContainer == null)
                {
                    var builder = new ContainerBuilder();

                    builder.RegisterInstance(BiggyConfiguration).As<IIdentityServer4BiggyConfiguration>();
                    builder.RegisterType<ClientStore>().As<IFullClientStore>();

                    var container = builder.Build();

                    _autofacContainer = container;
                }

                return _autofacContainer;
            }
        }

        public T Resolve<T>()
        {
            AutofacContainer.Resolve<T>();
            return AutofacContainer.Resolve<T>();
        }
        public IEnumerable<T> ResolveMany<T>()
        {
            return Resolve<IEnumerable<T>>();
        }
    }
}