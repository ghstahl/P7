using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using P7.Core.Settings;
using P7.Core.Startup;

namespace WebApplication5.GraphQLOpts
{
    public class MyConfigureServicesRegistrant : ConfigureServicesRegistrant
    {
        public override void OnConfigureServices(IServiceCollection services)
        {
            services.Configure<FiltersGraphQLConfig>(Configuration.GetSection(FiltersGraphQLConfig.WellKnown_SectionName));
        }

        public MyConfigureServicesRegistrant(IConfigurationRoot configuration) : base(configuration)
        {
        }
    }
}
