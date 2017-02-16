using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using P7.IdentityServer4.Common.Stores;

namespace P7.IdentityServer4.Common.Extensions
{
    public static class IdentityServer4Extensions
    {
        /// <summary>
        /// Adds the in default Resource Store.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static ContainerBuilder AddDefaultResourceStore(this ContainerBuilder builder)
        {
            builder.RegisterType<DefaultResourcesStore>().As<IResourceStore>();
            return builder;
        }
        /// <summary>
        /// Adds the in Admin Resource Store.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static ContainerBuilder AddAdminResourceStore(this ContainerBuilder builder)
        {
            builder.RegisterType<AdminResourceStore>().As<IAdminResourceStore>();
            return builder;
        }
    }
}
