namespace P7.Core.Localization
{
    public interface IResourceFetcher
    {
        object GetResourceSet(ResourceQueryHandle input);
    }
}