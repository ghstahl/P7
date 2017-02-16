using Autofac;
using IdentityServer4.Stores;
using P7.IdentityServer4.Common;
using P7.IdentityServer4.Common.Stores;

namespace P7.IdentityServer4.BiggyStore.Extensions
{
    public static class IdentityServer4Extensions
    {
        /// <summary>
        /// Adds the in identityserver 4 client stores.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static ContainerBuilder AddIdentityServer4BiggyClientStores(this ContainerBuilder builder)
        {
            builder.RegisterType<ClientStore>().As<IFullClientStore>();
            builder.RegisterType<ClientStore>().As<IClientStore>();
            return builder;
        }
        /// <summary>
        /// Adds the in Persisted Grant Store.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static ContainerBuilder AddIdentityServer4BiggyPersistedGrantStore(this ContainerBuilder builder)
        {
            builder.RegisterType<PersistedGrantStore>().As<IPersistedGrantStore>();
            return builder;
        }
        /// <summary>
        /// Adds the in Persisted Grant Store.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static ContainerBuilder AddIdentityServer4BiggyResourceStores(this ContainerBuilder builder)
        {
            builder.RegisterType<IdentityResourceStore>().As<IIdentityResourceStore>();
            builder.RegisterType<ApiResourceStore>().As<IApiResourceStore>();
            return builder;
        }
    }
}
