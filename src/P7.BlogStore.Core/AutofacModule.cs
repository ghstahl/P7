using Autofac;
using P7.BlogStore.Core.GraphQL;

namespace P7.BlogStore.Core
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BlogQueryInput>();
            builder.RegisterType<BlogsQueryInput>();
            builder.RegisterType<BlogMutationInput>();
            builder.RegisterType<BlogMetaDataType>();
            builder.RegisterType<Blog>();
            builder.RegisterType<BlogComment>();
            builder.RegisterType<BlogType>();
            builder.RegisterType<BlogPage>();
            builder.RegisterType<BlogPageType>();



        }
    }
}