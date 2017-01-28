namespace P7.BlogStore.Hugo
{
    public interface IBiggyConfiguration
    {
        string DatabaseName { get;  }
        string CollectionName { get; }
        string FolderStorage { get; }
    }
}