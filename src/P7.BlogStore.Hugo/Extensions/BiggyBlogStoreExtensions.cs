using Autofac;
using P7.HugoStore.Core;

namespace P7.BlogStore.Hugo.Extensions
{
    public static class BiggyBlogStoreExtensions
    {
        /// <summary>
        /// Adds BlogStore biggy configuration.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="dbPath">The full path to where biggy store the data.</param>
        /// <returns></returns>
        public static ContainerBuilder AddBlogStoreBiggyConfiguration(this ContainerBuilder builder, string dbPath)
        {
            var globalTenantDatabaseBiggyConfig = new TenantDatabaseBiggyConfig();
            globalTenantDatabaseBiggyConfig.UsingFolder(dbPath);
            globalTenantDatabaseBiggyConfig.UsingTenantId(TenantDatabaseBiggyConfig.GlobalTenantId);
            IBlogStoreBiggyConfiguration biggyConfiguration = new MyBiggyConfiguration()
            {
                FolderStorage = globalTenantDatabaseBiggyConfig.Folder,
                DatabaseName = globalTenantDatabaseBiggyConfig.Database
            };

            builder.Register(c => biggyConfiguration)
                .As<IBlogStoreBiggyConfiguration>()
                .SingleInstance();
            return builder;
        }
    }
}