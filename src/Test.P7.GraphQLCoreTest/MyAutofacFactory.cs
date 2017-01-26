using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using FakeItEasy;
using GraphQL;
using GraphQL.Http;
using GraphQL.Types;
using Microsoft.AspNetCore.Hosting;
using P7.GraphQLCore;
using Microsoft.Extensions.DependencyModel;
using Microsoft.DotNet.InternalAbstractions;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using P7.Core;

namespace Test.P7.GraphQLCoreTest
{
    public class MyAutofacFactory
    {
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
                        Assembly.Load(new AssemblyName("P7.GraphQLCore"))
                    };



                    var builder = new ContainerBuilder();


                    builder.RegisterInstance(Global.HostingEnvironment).As<IHostingEnvironment>();
                 
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