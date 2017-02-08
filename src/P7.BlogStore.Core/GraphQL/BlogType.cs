using GraphQL.Types;
using P7.Store;

namespace P7.BlogStore.Core.GraphQL
{
    public class BlogType : ObjectGraphType<Blog>
    {
        public BlogType()
        {
            Field(x => x.Id).Description("The Id of the Blog.");
            Field(x => x.Title).Description("The Title of the Blog.");
            Field(x => x.Summary).Description("The Summary of the Blog.");
            Field<ListGraphType<StringGraphType>>("categories", "The Categories of the Blog.");
            Field<ListGraphType<StringGraphType>>("tags", "The Tags of the Blog.");
            Field<BlogMetaDataType>("metaData", "The MetaData of the Blog.");
            Field(x => x.TimeStamp).Description("The TimeStamp of the Blog.");
            Field(x => x.Data).Description("The TimeStamp of the Blog.");
        }
    }
}