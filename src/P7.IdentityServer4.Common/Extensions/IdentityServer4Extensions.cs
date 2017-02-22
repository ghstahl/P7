using Autofac;
using IdentityServer4.Services;
using IdentityServer4.Stores;

namespace P7.IdentityServer4.Common.Extensions
{
    public static class IdentityServer4Extensions
    {
        /// <summary>
        /// Adds the in default Resource Store.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static ContainerBuilder AddResourceService<T>(this ContainerBuilder builder)
             where T : class, IResourceStore
        {
            builder.RegisterType<T>().As<IResourceStore>();
            return builder;
        }
        /// <summary>
        /// Adds the profile service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static ContainerBuilder AddProfileService<T>(this ContainerBuilder builder)
             where T : class, IProfileService
        {
            builder.RegisterType<T>().As<IProfileService>();
            return builder;
        }
        /// <summary>
        /// Adds the in Admin Resource Service.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static ContainerBuilder AddAdminResourceService<T>(this ContainerBuilder builder)
             where T : class, IAdminResourceStore
        {
            builder.RegisterType<T>().As<IAdminResourceStore>();
            return builder;
        }

        /// <summary>
        /// Adds the in Cors Policy Service.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static ContainerBuilder AddCorsPolicyService<T>(this ContainerBuilder builder)
              where T : class, ICorsPolicyService
        {
            builder.RegisterType<T>().As<ICorsPolicyService>();
            return builder;
        }

    }
}

