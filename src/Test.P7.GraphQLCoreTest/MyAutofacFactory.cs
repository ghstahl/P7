using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Autofac;
using FakeItEasy;
using GraphQL;
using GraphQL.Http;
using GraphQL.Types;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using P7.GraphQLCore;
using Microsoft.Extensions.DependencyModel;
using Microsoft.DotNet.InternalAbstractions;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using P7.BlogStore.Hugo;
using P7.Core;

namespace Test.P7.GraphQLCoreTest
{
    public class MyAutofacFactory
    {
        public IBiggyConfiguration BiggyConfiguration { get; set; }

        private IContainer _autofacContainer;
        public IContainer AutofacContainer
        {
            get
            {
                if (_autofacContainer == null)
                {
                    List<Assembly> assemblies = new List<Assembly>
                    {
                        Assembly.Load(new AssemblyName("P7.Core")),
                        Assembly.Load(new AssemblyName("P7.Globalization")),
                        Assembly.Load(new AssemblyName("P7.BlogStore.Hugo")),
                        Assembly.Load(new AssemblyName("P7.GraphQLCore")),
                        Assembly.Load(new AssemblyName("P7.BlogStore.Core"))
                    };



                    var builder = new ContainerBuilder();

                    builder.RegisterInstance(BiggyConfiguration).As<IBiggyConfiguration>();

                    builder.RegisterInstance(Global.HostingEnvironment).As<IHostingEnvironment>();
                    var httpContextAccessor = A.Fake<IHttpContextAccessor>();
                    var httpContext = A.Fake<HttpContext>();
                    var featureCollection = A.Fake<IFeatureCollection>();
                    var requestCultureFeature = A.Fake<IRequestCultureFeature>();
                    var requestCulture = new RequestCulture(new CultureInfo("en-US"));
                    A.CallTo(() => httpContextAccessor.HttpContext).Returns(httpContext);
                    A.CallTo(() => httpContext.Features).Returns(featureCollection);
                    A.CallTo(() => featureCollection.Get<IRequestCultureFeature>()).Returns(requestCultureFeature);
                    A.CallTo(() => requestCultureFeature.RequestCulture).Returns(requestCulture);





                    builder.RegisterInstance(httpContextAccessor).As<IHttpContextAccessor>();
                    builder.RegisterType<GraphQLUserContext>();

                    builder.RegisterAssemblyModules(assemblies.ToArray());

                    var locOptions = new LocalizationOptions();
                    var options = A.Fake<IOptions<LocalizationOptions>>();
                    A.CallTo(() => options.Value).Returns(locOptions);

                    builder.RegisterInstance(options).As<IOptions<LocalizationOptions>>();
                    builder.RegisterType<ResourceManagerStringLocalizerFactory>()
                        .As<IStringLocalizerFactory>()
                        .SingleInstance();
                    builder.RegisterType<MemoryCacheOptions>()
                        .As<IOptions<MemoryCacheOptions>>();
                    builder.RegisterType<MemoryCache>()
                       .As<IMemoryCache>();


                    var container = builder.Build();

                    _autofacContainer = container;
                }

                return _autofacContainer;
            }
        }

        public T Resolve<T>()
        {
            return AutofacContainer.Resolve<T>();
        }

    }
}