using Autofac;
using P7.IdentityServer4.Common.Extensions;

namespace P7.IdentityServer4.Common
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.AddDefaultResourceStore();
            builder.AddAdminResourceStore();
            builder.AddCorsPolicyService();
        }
    }
}