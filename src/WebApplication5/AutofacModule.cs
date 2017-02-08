﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.Logging;
using P7.Core.Middleware;
using P7.Core.Providers;
using P7.GraphQLCore.Validators;
using P7.SimpleRedirect.Core;
using WebApplication5.GraphQLOpts;
using WebApplication5.Services;

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

        }
    }
}
