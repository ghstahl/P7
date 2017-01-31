using System;
using GraphQL;
using GraphQL.Types;
using P7.GraphQLCore;

namespace P7.BlogStore.Core.GraphQL
{
    public class MyQueryFieldRecordRegistrationBase : QueryFieldRecordRegistrationBase
    {
        private IBlogStore _blogStore;
        public MyQueryFieldRecordRegistrationBase(
            IBlogStore blogStore)
        {
            _blogStore = blogStore;
        }



        protected override void PopulateStringGraphTypes()
        {
            ListStringGraphTypeFieldRecords.Add(new FieldRecord<StringGraphType>()
            {
                Name = "blog",
                QueryArguments = new QueryArguments(new QueryArgument<BlogQueryInput> {Name = "input"}),

                Resolve = async context =>
                {
                    try
                    {
                        var userContext = context.UserContext.As<GraphQLUserContext>();
                        var blog = context.GetArgument<Blog>("input");
                        var result = await _blogStore.FetchAsync(blog.Id);
                        return result;
                    }
                    catch (Exception e)
                    {

                    }
                    return null;
                    //                    return await Task.Run(() => { return ""; });
                }
            });
            ListStringGraphTypeFieldRecords.Add(new FieldRecord<StringGraphType>()
            {
                Name = "blogs",
                QueryArguments = new QueryArguments(new QueryArgument<BlogsQueryInput> { Name = "input" }),

                Resolve = async context =>
                {
                    try
                    {
                        var userContext = context.UserContext.As<GraphQLUserContext>();
                        var blogsPageHandle = context.GetArgument<BlogsPageHandle>("input");

                        var result = await _blogStore.FetchAsync(Guid.Empty);
                        return result;
                    }
                    catch (Exception e)
                    {

                    }
                    return null;
                    //                    return await Task.Run(() => { return ""; });
                }
            });
        }
    }
}
