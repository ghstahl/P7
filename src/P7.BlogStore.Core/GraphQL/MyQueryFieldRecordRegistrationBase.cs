using System;
using GraphQL;
using GraphQL.Types;
using P7.GraphQLCore;
using P7.Store;

namespace P7.BlogStore.Core.GraphQL
{
    public class MyQueryFieldRecordRegistrationBase : IQueryFieldRecordRegistration
    {
        private IBlogStore _blogStore;

        public MyQueryFieldRecordRegistrationBase(
            IBlogStore blogStore)
        {
            _blogStore = blogStore;
        }

        public  void AddGraphTypeFields(QueryCore queryCore)
        {
            queryCore.FieldAsync<StringGraphType>(name: "blog",
                description: null,
                arguments: new QueryArguments(new QueryArgument<BlogQueryInput> {Name = "input"}),
                resolve: async context =>
                {
                    try
                    {
                        var userContext = context.UserContext.As<GraphQLUserContext>();
                        var blog = context.GetArgument<Blog>("input");
                        var result = await _blogStore.FetchAsync(blog.Id_G);
                        return result;
                    }
                    catch (Exception e)
                    {

                    }
                    return null;
                    //                    return await Task.Run(() => { return ""; });
                },
                deprecationReason: null);
            queryCore.FieldAsync<StringGraphType>(name: "blogs",
                description: null,
                arguments: new QueryArguments(new QueryArgument<BlogsQueryInput> {Name = "input"}),
                resolve: async context =>
                {
                    try
                    {
                        var userContext = context.UserContext.As<GraphQLUserContext>();
                        var blogsPageHandle = context.GetArgument<BlogsPageHandle>("input");

                        var pagingState = blogsPageHandle.PagingState.SafeConvertFromBase64String();

                        var categories = blogsPageHandle.Categories?.ToArray();
                        var tags = blogsPageHandle.Tags?.ToArray();
                        DateTime baseDateTime = new DateTime();
                        DateTime? timeStampLowerBoundary = baseDateTime == blogsPageHandle.TimeStampLowerBoundary
                            ? (DateTime?) null
                            : blogsPageHandle.TimeStampLowerBoundary;
                        DateTime? timeStampUpperBoundary = baseDateTime == blogsPageHandle.TimeStampUpperBoundary
                            ? (DateTime?) null
                            : blogsPageHandle.TimeStampUpperBoundary;
                        var result = await _blogStore.PageAsync(
                            blogsPageHandle.PageSize,
                            pagingState,
                            timeStampLowerBoundary,
                            timeStampUpperBoundary,
                            categories,
                            tags);

                        return result;
                    }
                    catch (Exception e)
                    {

                    }
                    return null;
                    //                    return await Task.Run(() => { return ""; });
                },
                deprecationReason: null);
        }
    }
}
