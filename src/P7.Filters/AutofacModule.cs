using System.Linq;
using System.Reflection;
using Autofac;
using Microsoft.AspNetCore.Mvc.Filters;
using P7.Core.Middleware;
using P7.Core.Reflection;
using Serilog;
using Module = Autofac.Module;

namespace P7.Filters
{
    public class AutofacModule : Module
    {
        static ILogger logger = Log.ForContext<AutofacModule>();
        protected override void Load(ContainerBuilder builder)
        {
            logger.Information("Hi from pingo.filters Autofac.Load!");
            var assembly = this.GetType().GetTypeInfo().Assembly;
            var derivedTypes = TypeHelper<ActionFilterAttribute>.FindDerivedTypes(assembly).ToArray();
            var derivedTypesName = derivedTypes.Select(x => x.GetTypeInfo().Name);
            logger.Information("Found these types: {DerivedTypes}", derivedTypesName);

            builder.RegisterTypes(derivedTypes).SingleInstance();


            derivedTypes = TypeHelper<MiddlewarePlugin>.FindDerivedTypes(assembly).ToArray();
            derivedTypesName = derivedTypes.Select(x => x.GetTypeInfo().Name);
            logger.Information("Found these types: {DerivedTypes}", derivedTypesName);
            builder.RegisterTypes(derivedTypes).SingleInstance();


            /*

            builder.RegisterType<AuthActionFilter>().SingleInstance();
            builder.RegisterType<LogFilter>().SingleInstance();
            builder.RegisterType<LogFilter2>().SingleInstance();
            builder.RegisterType<LogFilter3>().SingleInstance();
            */
        }
    }
}
