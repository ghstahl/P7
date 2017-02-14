using Autofac;
using IdentityServer4.Stores;
using P7.IdentityServer4.Common;
using P7.IdentityServer4.Common.Stores;

namespace P7.IdentityServer4.BiggyStore
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DefaultResourcesStore>().As<IResourceStore>();
        }
    }
}