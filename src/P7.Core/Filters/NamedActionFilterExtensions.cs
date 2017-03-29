using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Autofac;
using Microsoft.AspNetCore.Mvc.Filters;
using P7.Core.Reflection;

namespace P7.Core.Filters
{
    public static class NamedActionFilterExtensions
    {
        public static void RegisterNamedActionFilter<T>(this ContainerBuilder builder, string name = null) where T:class
        {
            if (name == null)
            {
                var type = typeof(T);
                name = type.AssemblyQualifiedNameWithoutVersion();
            }    
            builder.RegisterType<T>().Named<IActionFilter>(name);
        }
    }
}
