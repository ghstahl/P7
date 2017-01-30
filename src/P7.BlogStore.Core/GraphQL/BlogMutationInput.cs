using GraphQL.Types;

namespace P7.BlogStore.Core.GraphQL
{
    public class BlogMutationInput : InputObjectGraphType
    {
        public BlogMutationInput()
        {
            Name = "BlogMutationInput";
            Field<NonNullGraphType<StringGraphType>>("id");
            Field<NonNullGraphType<StringGraphType>>("data");
            Field<NonNullGraphType<BlogMetaDataType>>("metaData");
            Field<NonNullGraphType<DateGraphType>>("timeStamp");
            Field<ListGraphType<StringGraphType>>("tags");
            Field<ListGraphType<StringGraphType>>("categories");
        }
    }
}