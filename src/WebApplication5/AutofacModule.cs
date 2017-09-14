using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using P7.Core;
using P7.Core.Middleware;
using P7.Core.Providers;
using P7.GraphQLCore.Validators;
using P7.SimpleRedirect.Core;
using WebApplication5.GraphQLOpts;
using WebApplication5.Services;
using P7.External.SPA.Core;
using P7.RazorProvider.Store.Core;
using P7.RazorProvider.Store.Core.Interfaces;

namespace WebApplication5
{
    class InMemorySimpleRedirectStore : ISimpleRedirectorStore
    {
        private List<SimpleRedirectRecord> _simpleRedirectRecords;

        private List<SimpleRedirectRecord> Records
        {
            get
            {
                if (_simpleRedirectRecords == null)
                {
                    _simpleRedirectRecords = new List<SimpleRedirectRecord>()
                    {
                        new SimpleRedirectRecord() {BaseUrl = "www.google.com", Key = "google", Scheme = "https"},
                        new SimpleRedirectRecord() {BaseUrl = "www.facebook.com", Key = "facebook", Scheme = "https"},
                        new SimpleRedirectRecord() {BaseUrl = "www.microsoft.com", Key = "microsoft", Scheme = "https"}
                    };
                }
                return _simpleRedirectRecords;
            }
        }

        public async Task<SimpleRedirectRecord> FetchRedirectRecord(string key)
        {
            var query = from item in Records
                where item.Key == key
                select item;
            return query.FirstOrDefault();
        }
    }

    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // The generic ILogger<TCategoryName> service was added to the ServiceCollection by ASP.NET Core.
            // It was then registered with Autofac using the Populate method in ConfigureServices.
            builder.Register(c => new ValuesService(c.Resolve<ILogger<ValuesService>>()))
                .As<IValuesService>()
                .InstancePerLifetimeScope();
            builder.Register(c => new InMemorySimpleRedirectStore())
                .As<ISimpleRedirectorStore>()
                .SingleInstance();

            builder.RegisterType<LocalSettingsGlobalPathAuthorizeStore>()
                .As<IGlobalPathAuthorizeStore>()
                .SingleInstance();
            builder.RegisterType<LocalSettingsOptOutOptInAuthorizeStore>()
                .As<IOptOutOptInAuthorizeStore>()
                .SingleInstance();

            builder.RegisterType<OptOutOptInFilterProvider>()
                .As<IFilterProvider>()
                .SingleInstance();

            // register the global configuration root
            builder.RegisterType<GlobalConfigurationRoot>()
                .As<IConfigurationRoot>()
                .SingleInstance();

            // build external InMemoryStore
            var remoteStaticExternalSpaStore = new RemoteStaticExternalSpaStore(
                "https://rawgit.com/ghstahl/P7/master/src/WebApplication5/external.spa.config.json");
            var records = remoteStaticExternalSpaStore.GetRemoteDataAsync().GetAwaiter().GetResult();
            foreach (var spa in records.Spas)
            {
                remoteStaticExternalSpaStore.AddRecord(spa);
            }

            
            /*
            remoteStaticExternalSpaStore.AddRecord(new ExternalSPARecord()
            {
                Key = "Support",
                RequireAuth = false,
                RenderTemplate = "<div access_token={%{user.access_token}%}>Well Hello Support</div>"
            });
            remoteStaticExternalSpaStore.AddRecord(new ExternalSPARecord()
            {
                Key = "admin",
                RequireAuth = true,
                RenderTemplate = "<div access_token={%{user.access_token}%}>Well Hello Admin</div>"
            });
            */

            builder.Register(c => remoteStaticExternalSpaStore)
                .As<IExternalSPAStore>()
                .SingleInstance();

        }
    }
}
