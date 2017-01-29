using Autofac;
using P7.BlogStore.Core;
using P7.BlogStore.Hugo.GraphQL;

namespace P7.BlogStore.Hugo
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HugoBlogStore>().As<IBlogStore>();
            builder.RegisterType<BlogQueryInput>();
        }
    }
}