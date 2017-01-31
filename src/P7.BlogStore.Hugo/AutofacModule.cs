using Autofac;
using Hugo.Data.Json;
using P7.BlogStore.Core;

namespace P7.BlogStore.Hugo
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BlogSorter>().As<ISorter<Blog>>();
            builder.RegisterType<HugoBlogStore>().As<IBlogStore>();
            builder.RegisterType<BlogCommentSorter>().As<ISorter<BlogComment>>();
            builder.RegisterType<HugoBlogCommentStore>().As<IBlogCommentStore>();   
        }
    }
}