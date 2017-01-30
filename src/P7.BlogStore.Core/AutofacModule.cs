using Autofac;
using P7.BlogStore.Core.GraphQL;

namespace P7.BlogStore.Core
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BlogQueryInput>();
            builder.RegisterType<BlogMutationInput>();
            builder.RegisterType<BlogMetaDataType>();
        }
    }
}