using Autofac;
using P7.HugoStore.Core;

namespace P7.RazorProvider.Store.Hugo.Extensions
{
    public static class BiggyRazorLocationStoreExtensions
    {
        /// <summary>
        /// Adds BlogStore biggy configuration.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="dbPath">The full path to where biggy store the data.</param>
        /// <param name="tentantId">The tenantId of this store.</param>
        /// <returns></returns>
        public static ContainerBuilder AddRazorLocationStoreBiggyConfiguration(this ContainerBuilder builder, string dbPath,string tentantId)
        {
            var globalTenantDatabaseBiggyConfig = new TenantDatabaseBiggyConfig();
            globalTenantDatabaseBiggyConfig.UsingFolder(dbPath);
            globalTenantDatabaseBiggyConfig.UsingTenantId(TenantDatabaseBiggyConfig.GlobalTenantId);
            IRazorLocationStoreBiggyConfiguration biggyConfiguration = new MyBiggyConfiguration()
            {
                FolderStorage = globalTenantDatabaseBiggyConfig.Folder,
                DatabaseName = globalTenantDatabaseBiggyConfig.Database,
                TenantId = tentantId
            };

            builder.Register(c => biggyConfiguration)
                .As<IRazorLocationStoreBiggyConfiguration>()
                .SingleInstance();
            return builder;
        }
    }
}