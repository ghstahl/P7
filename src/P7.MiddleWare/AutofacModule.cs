using Autofac;

namespace P7.MiddleWare
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<P7RewriteMiddleware>();
        }
    }
}