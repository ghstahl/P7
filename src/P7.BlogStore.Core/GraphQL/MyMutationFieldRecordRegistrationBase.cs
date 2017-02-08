using System;
using GraphQL;
using GraphQL.Types;
using P7.GraphQLCore;

namespace P7.BlogStore.Core.GraphQL
{
    public class MyMutationFieldRecordRegistrationBase : IMutationFieldRecordRegistration
    {
        private IBlogStore _blogStore;

        public MyMutationFieldRecordRegistrationBase(
            IBlogStore blogStore)
        {
            _blogStore = blogStore;
        }

        public void AddGraphTypeFields(MutationCore mutationCore)
        {
            mutationCore.FieldAsync<StringGraphType>(name: "blog",
                description: null,
                arguments: new QueryArguments(new QueryArgument<BlogMutationInput> {Name = "input"}),
                resolve: async context =>
                {
                    try
                    {
                        var userContext = context.UserContext.As<GraphQLUserContext>();
                        var blog = context.GetArgument<Blog>("input");
                        await _blogStore.InsertAsync(blog);
                        return true;
                    }
                    catch (Exception e)
                    {

                    }
                    return false;
                    //                    return await Task.Run(() => { return ""; });

                },
                deprecationReason: null
            );
        }
    }
}