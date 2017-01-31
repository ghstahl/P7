namespace P7.BlogStore.Core
{
    public class BlogMetaData
    {
        public string Category { get; set; }
        public string Version { get; set; }
        public override bool Equals(object obj)
        {
            var other = obj as BlogMetaData;
            if (other == null)
            {
                return false;
            }
            if (!Category.IsEqual(other.Category))
            {
                return false;
            }
            if (!Version.IsEqual(other.Version))
            {
                return false;
            }
            return true;
        }
    }
}