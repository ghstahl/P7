﻿using Autofac;
using P7.GraphQLCore;

namespace P7.Globalization
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ResourceQueryInput>();
            builder.RegisterType<ResourceFetcher>().As<IResourceFetcher>();
            //   builder.RegisterType<MyFieldRecordRegistration>().As<IFieldRecordRegistration>();
            //   builder.RegisterType<MyFieldRecordRegistration2>().As<IFieldRecordRegistration>();
        }
    }
}