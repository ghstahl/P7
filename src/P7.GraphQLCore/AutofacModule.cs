using System;
using Autofac;
using GraphQL;
using GraphQL.Execution;
using GraphQL.Http;
using GraphQL.Types;
using GraphQL.Validation;
using GraphQL.Validation.Complexity;
using P7.Core.Reflection;
using P7.GraphQLCore.Validators;

namespace P7.GraphQLCore
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            // This is a global sweep to find all types that
            // implement IMutationFieldRecordRegistration and IQueryFieldRecordRegistration.
            // We then register every one of them.
            // Future would be to database this, but for now if it is referenced it is in.
            var myTypes = TypeHelper<IQueryFieldRecordRegistration>
                .FindTypesInAssemblies(TypeHelper<IQueryFieldRecordRegistration>.IsType);
            foreach (var type in myTypes)
            {
                builder.RegisterType(type).As<IQueryFieldRecordRegistration>();
            }
            builder.RegisterType<QueryFieldRecordRegistrationStore>()
                .As<IQueryFieldRecordRegistrationStore>()
                .SingleInstance();

            myTypes = TypeHelper<IMutationFieldRecordRegistration>
                .FindTypesInAssemblies(TypeHelper<IMutationFieldRecordRegistration>.IsType);
            foreach (var type in myTypes)
            {
                builder.RegisterType(type).As<IMutationFieldRecordRegistration>();
            }
            builder.RegisterType<MutationFieldRecordRegistrationStore>()
                .As<IMutationFieldRecordRegistrationStore>()
                .SingleInstance();





            builder.RegisterType<GraphQLDocumentBuilder>().As<IDocumentBuilder>();
            builder.RegisterType<DocumentValidator>().As<IDocumentValidator>();
            builder.RegisterType<ComplexityAnalyzer>().As<IComplexityAnalyzer>();
            builder.RegisterType<DocumentExecuter>().As<IDocumentExecuter>().SingleInstance();
            builder.RegisterInstance(new DocumentWriter(indent: true)).As<IDocumentWriter>();
            builder.RegisterType<QueryCore>().AsSelf();
            builder.RegisterType<MutationCore>().AsSelf();
            builder.RegisterType<SchemaCore>().As<ISchema>();

            builder.Register<Func<Type, GraphType>>(c =>
            {
                var context = c.Resolve<IComponentContext>();
                return t => {
                    var res = context.Resolve(t);
                    return (GraphType)res;
                };
            });

            /*
             * this autofac module is reserved for things that everyone gets.
             * This IRequiresAuthValidationRuleConfig has to be done in your unit test code or
             * your main application's registrations.
            builder.RegisterType<InMemoryRequiresAuthValidationRuleConfig>()
                .As<IRequiresAuthValidationRuleConfig>()
                .SingleInstance();
                */
            builder.RegisterType<TestValidationRule>()
                .As<IValidationRule>()
                .SingleInstance();
        }
    }
}