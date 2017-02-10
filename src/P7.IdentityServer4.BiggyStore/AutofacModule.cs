using Autofac;
using IdentityServer4.Stores;

namespace P7.IdentityServer4.BiggyStore
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ClientStore>().As<IClientStore>();
        }
    }
}