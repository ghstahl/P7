using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Localization;
using P7.BlogStore.Core;
using P7.Core.Localization;
using P7.GraphQLCore;

namespace P7.BlogStore.Hugo.GraphQL
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
                    var userContext = context.UserContext.As<GraphQLUserContext>();
                    var rqf = userContext.HttpContextAccessor.HttpContext.Features.Get<IRequestCultureFeature>();
                    // Culture contains the information of the requested culture
                    CultureInfo currentCulture = rqf.RequestCulture.Culture;

                    var input = context.GetArgument<ResourceQueryHandle>("input");

                    if (!string.IsNullOrEmpty(input.Culture))
                    {
                        try
                        {
                            currentCulture = new CultureInfo(input.Culture);
                        }
                        catch (Exception)
                        {
                            currentCulture = new CultureInfo("en-US");
                        }
                    }
                    /*
                    var obj = await _resourceFetcher.GetResourceSetAsync(
                        new ResourceQueryHandle()
                        {
                            Culture = currentCulture.Name,
                            Id = input.Id,
                            Treatment = input.Treatment
                        });
                    return obj;
                    */
                    return await Task.Run(()=> { return ""; });
                }
            });
        }
    }
}
