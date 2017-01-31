using GraphQL.Types;

namespace P7.BlogStore.Core.GraphQL
{
    public class BlogsQueryInput : InputObjectGraphType
    {
        public BlogsQueryInput()
        {
            Name = "BlogsQueryInput";
            Field<NonNullGraphType<IntGraphType>>("pageSize");
            Field<StringGraphType>("pagingState");
            Field<DateGraphType>("timestampLowerBoundary");
            Field<DateGraphType>("timestampUpperBoundary");
            Field<ListGraphType<StringGraphType>>("categories");
            Field<ListGraphType<StringGraphType>>("tags");
        }
    }
}