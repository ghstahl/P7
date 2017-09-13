using Autofac;
using P7.RazorProvider.Store.Hugo.Interfaces;

namespace P7.RazorProvider.Store.Hugo
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HugoRazorLocationStore>().As<IRazorLocationStore>();
        }
    }
}