using GraphQL.Types;

namespace P7.BlogStore.Core.GraphQL
{
    public class BlogPageType : ObjectGraphType<BlogPage>
    {
        public BlogPageType()
        {
            Field(x => x.CurrentPagingState).Description("The current paging state of the this request for blog entries.");
            Field(x => x.PagingState).Description("The next paging state of the subsequent request for blog entries.");
            Field<ListGraphType<BlogType>>("blogs", "The blogs.");
 
        }
    }
}