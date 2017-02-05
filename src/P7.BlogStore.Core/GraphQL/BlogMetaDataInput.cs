using GraphQL.Types;

namespace P7.BlogStore.Core.GraphQL
{
    public class BlogMetaDataInput : InputObjectGraphType
    {
        public BlogMetaDataInput()
        {
            Name = "BlogMetaDataInput";
            Field<NonNullGraphType<StringGraphType>>("category");
            Field<NonNullGraphType<StringGraphType>>("version");
        }
    }
}