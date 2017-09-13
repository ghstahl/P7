namespace P7.RazorProvider.Store.Hugo.Extensions
{
    internal class MyBiggyConfiguration : IRazorLocationStoreBiggyConfiguration
    {
        public string DatabaseName { get; set; }
        public string FolderStorage { get; set; }
        public string TenantId { get; set; }
    }
}
