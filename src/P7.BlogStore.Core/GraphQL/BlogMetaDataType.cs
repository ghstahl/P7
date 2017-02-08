using System.Collections.Generic;
using GraphQL.Language.AST;
using GraphQL.Types;
using Newtonsoft.Json;

namespace P7.BlogStore.Core.GraphQL
{

    public class BlogMetaDataType : ObjectGraphType<BlogMetaData>
    {
        public BlogMetaDataType()
        {
            Name = "BlogMetaDataType";
            Field(x => x.Category).Description("The Category of the BlogMetaData.");
            Field(x => x.Version).Description("The Version of the BlogMetaData.");
        }
    }
}