using System;
using System.Threading.Tasks;
using GraphQL.Types;

namespace P7.GraphQLCore
{
    public class SchemaCore : Schema
    {
        public SchemaCore(Func<Type, GraphType> resolveType)
            : base(resolveType)
        {
            Query = (QueryCore)resolveType(typeof(QueryCore));
            Mutation = (MutationCore)resolveType(typeof(MutationCore));
        }
    }
    public class MutationCore : ObjectGraphType<object>
    {
        public MutationCore(IMutationFieldRecordRegistrationStore fieldStore)
        {
            Name = "Mutation";

            var stringGraphTypeFieldRecords = fieldStore.GetStringGraphTypes();
            if (stringGraphTypeFieldRecords != null)
            {
                foreach (var stringTypeItem in stringGraphTypeFieldRecords)
                {
                    FieldRecord<StringGraphType> record = stringTypeItem;

                    FieldAsync<StringGraphType>(name: record.Name,
                        description: record.Description,
                        arguments: record.QueryArguments,
                        resolve: record.Resolve,
                        deprecationReason: record.DeprecationReason);

                }
            }

            var booleanGraphTypeFieldRecords = fieldStore.GetBooleanGraphTypes();
            if (booleanGraphTypeFieldRecords != null)
            {
                foreach (var booleanTypeItem in booleanGraphTypeFieldRecords)
                {
                    FieldRecord<BooleanGraphType> record = booleanTypeItem;
                    Func<ResolveFieldContext<object>, Task<object>> stringTypeResolver =
                        record.Resolve;

                    FieldAsync<BooleanGraphType>(name: record.Name,
                        description: record.Description,
                        arguments: record.QueryArguments,
                        resolve: stringTypeResolver,
                        deprecationReason: record.DeprecationReason);

                }
            }

            var dateGraphTypeFieldRecords = fieldStore.GetDateGraphTypes();
            if (dateGraphTypeFieldRecords != null)
            {
                foreach (var dateTypeItem in dateGraphTypeFieldRecords)
                {
                    FieldRecord<DateGraphType> record = dateTypeItem;
                    Func<ResolveFieldContext<object>, Task<object>> stringTypeResolver =
                        record.Resolve;

                    FieldAsync<DateGraphType>(name: record.Name,
                        description: record.Description,
                        arguments: record.QueryArguments,
                        resolve: stringTypeResolver,
                        deprecationReason: record.DeprecationReason);

                }
            }

            var decimalGraphTypeFieldRecords = fieldStore.GetDecimalGraphTypes();
            if (decimalGraphTypeFieldRecords != null)
            {
                foreach (var decimalTypeItem in decimalGraphTypeFieldRecords)
                {
                    FieldRecord<DecimalGraphType> record = decimalTypeItem;
                    Func<ResolveFieldContext<object>, Task<object>> stringTypeResolver =
                        record.Resolve;

                    FieldAsync<DecimalGraphType>(name: record.Name,
                        description: record.Description,
                        arguments: record.QueryArguments,
                        resolve: stringTypeResolver,
                        deprecationReason: record.DeprecationReason);

                }
            }

            var enumerationGraphTypeFieldRecords = fieldStore.GetEnumerationGraphTypes();
            if (enumerationGraphTypeFieldRecords != null)
            {
                foreach (var enumerationTypeItem in enumerationGraphTypeFieldRecords)
                {
                    FieldRecord<EnumerationGraphType> record = enumerationTypeItem;
                    Func<ResolveFieldContext<object>, Task<object>> stringTypeResolver =
                        record.Resolve;

                    FieldAsync<EnumerationGraphType>(name: record.Name,
                        description: record.Description,
                        arguments: record.QueryArguments,
                        resolve: stringTypeResolver,
                        deprecationReason: record.DeprecationReason);

                }
            }

            var floatGraphTypeFieldRecords = fieldStore.GetFloatGraphTypes();
            if (floatGraphTypeFieldRecords != null)
            {
                foreach (var floatTypeItem in floatGraphTypeFieldRecords)
                {
                    FieldRecord<FloatGraphType> record = floatTypeItem;
                    Func<ResolveFieldContext<object>, Task<object>> stringTypeResolver =
                        record.Resolve;

                    FieldAsync<FloatGraphType>(name: record.Name,
                        description: record.Description,
                        arguments: record.QueryArguments,
                        resolve: stringTypeResolver,
                        deprecationReason: record.DeprecationReason);

                }
            }

            var intGraphTypeFieldRecords = fieldStore.GetIntGraphTypeTypes();
            if (intGraphTypeFieldRecords != null)
            {
                foreach (var intTypeItem in intGraphTypeFieldRecords)
                {
                    FieldRecord<IntGraphType> record = intTypeItem;
                    Func<ResolveFieldContext<object>, Task<object>> stringTypeResolver =
                        record.Resolve;

                    FieldAsync<IntGraphType>(name: record.Name,
                        description: record.Description,
                        arguments: record.QueryArguments,
                        resolve: stringTypeResolver,
                        deprecationReason: record.DeprecationReason);

                }
            }

            var interfaceGraphTypeFieldRecords = fieldStore.GetInterfaceGraphTypes();
            if (intGraphTypeFieldRecords != null)
            {
                foreach (var interfaceTypeItem in interfaceGraphTypeFieldRecords)
                {
                    FieldRecord<InterfaceGraphType> record = interfaceTypeItem;
                    Func<ResolveFieldContext<object>, Task<object>> stringTypeResolver =
                        record.Resolve;

                    FieldAsync<InterfaceGraphType>(name: record.Name,
                        description: record.Description,
                        arguments: record.QueryArguments,
                        resolve: stringTypeResolver,
                        deprecationReason: record.DeprecationReason);

                }
            }

            var listGraphTypeFieldRecords = fieldStore.GetListGraphTypes();
            if (listGraphTypeFieldRecords != null)
            {
                foreach (var listTypeItem in listGraphTypeFieldRecords)
                {
                    FieldRecord<ListGraphType> record = listTypeItem;
                    Func<ResolveFieldContext<object>, Task<object>> stringTypeResolver =
                        record.Resolve;

                    FieldAsync<ListGraphType>(name: record.Name,
                        description: record.Description,
                        arguments: record.QueryArguments,
                        resolve: stringTypeResolver,
                        deprecationReason: record.DeprecationReason);

                }
            }

            var unionGraphTypeFieldRecords = fieldStore.GetUnionGraphTypes();
            if (unionGraphTypeFieldRecords != null)
            {
                foreach (var unionTypeItem in unionGraphTypeFieldRecords)
                {
                    FieldRecord<UnionGraphType> record = unionTypeItem;
                    Func<ResolveFieldContext<object>, Task<object>> stringTypeResolver =
                        record.Resolve;

                    FieldAsync<UnionGraphType>(name: record.Name,
                        description: record.Description,
                        arguments: record.QueryArguments,
                        resolve: stringTypeResolver,
                        deprecationReason: record.DeprecationReason);

                }
            }
        }
    }
    public class QueryCore : ObjectGraphType<object>
    {
        public QueryCore(IQueryFieldRecordRegistrationStore fieldStore)
        {
            Name = "Query";

            var stringGraphTypeFieldRecords = fieldStore.GetStringGraphTypes();
            if (stringGraphTypeFieldRecords != null)
            {
                foreach (var stringTypeItem in stringGraphTypeFieldRecords)
                {
                    FieldRecord<StringGraphType> record = stringTypeItem;

                    FieldAsync<StringGraphType>(name: record.Name,
                        description: record.Description,
                        arguments: record.QueryArguments,
                        resolve: record.Resolve,
                        deprecationReason: record.DeprecationReason);

                }
            }

            var booleanGraphTypeFieldRecords = fieldStore.GetBooleanGraphTypes();
            if (booleanGraphTypeFieldRecords != null)
            {
                foreach (var booleanTypeItem in booleanGraphTypeFieldRecords)
                {
                    FieldRecord<BooleanGraphType> record = booleanTypeItem;
                    Func<ResolveFieldContext<object>, Task<object>> stringTypeResolver =
                        record.Resolve;

                    FieldAsync<BooleanGraphType>(name: record.Name,
                        description: record.Description,
                        arguments: record.QueryArguments,
                        resolve: stringTypeResolver,
                        deprecationReason: record.DeprecationReason);

                }
            }

            var dateGraphTypeFieldRecords = fieldStore.GetDateGraphTypes();
            if (dateGraphTypeFieldRecords != null)
            {
                foreach (var dateTypeItem in dateGraphTypeFieldRecords)
                {
                    FieldRecord<DateGraphType> record = dateTypeItem;
                    Func<ResolveFieldContext<object>, Task<object>> stringTypeResolver =
                        record.Resolve;

                    FieldAsync<DateGraphType>(name: record.Name,
                        description: record.Description,
                        arguments: record.QueryArguments,
                        resolve: stringTypeResolver,
                        deprecationReason: record.DeprecationReason);

                }
            }

            var decimalGraphTypeFieldRecords = fieldStore.GetDecimalGraphTypes();
            if (decimalGraphTypeFieldRecords != null)
            {
                foreach (var decimalTypeItem in decimalGraphTypeFieldRecords)
                {
                    FieldRecord<DecimalGraphType> record = decimalTypeItem;
                    Func<ResolveFieldContext<object>, Task<object>> stringTypeResolver =
                        record.Resolve;

                    FieldAsync<DecimalGraphType>(name: record.Name,
                        description: record.Description,
                        arguments: record.QueryArguments,
                        resolve: stringTypeResolver,
                        deprecationReason: record.DeprecationReason);

                }
            }

            var enumerationGraphTypeFieldRecords = fieldStore.GetEnumerationGraphTypes();
            if (enumerationGraphTypeFieldRecords != null)
            {
                foreach (var enumerationTypeItem in enumerationGraphTypeFieldRecords)
                {
                    FieldRecord<EnumerationGraphType> record = enumerationTypeItem;
                    Func<ResolveFieldContext<object>, Task<object>> stringTypeResolver =
                        record.Resolve;

                    FieldAsync<EnumerationGraphType>(name: record.Name,
                        description: record.Description,
                        arguments: record.QueryArguments,
                        resolve: stringTypeResolver,
                        deprecationReason: record.DeprecationReason);

                }
            }

            var floatGraphTypeFieldRecords = fieldStore.GetFloatGraphTypes();
            if (floatGraphTypeFieldRecords != null)
            {
                foreach (var floatTypeItem in floatGraphTypeFieldRecords)
                {
                    FieldRecord<FloatGraphType> record = floatTypeItem;
                    Func<ResolveFieldContext<object>, Task<object>> stringTypeResolver =
                        record.Resolve;

                    FieldAsync<FloatGraphType>(name: record.Name,
                        description: record.Description,
                        arguments: record.QueryArguments,
                        resolve: stringTypeResolver,
                        deprecationReason: record.DeprecationReason);

                }
            }

            var intGraphTypeFieldRecords = fieldStore.GetIntGraphTypeTypes();
            if (intGraphTypeFieldRecords != null)
            {
                foreach (var intTypeItem in intGraphTypeFieldRecords)
                {
                    FieldRecord<IntGraphType> record = intTypeItem;
                    Func<ResolveFieldContext<object>, Task<object>> stringTypeResolver =
                        record.Resolve;

                    FieldAsync<IntGraphType>(name: record.Name,
                        description: record.Description,
                        arguments: record.QueryArguments,
                        resolve: stringTypeResolver,
                        deprecationReason: record.DeprecationReason);

                }
            }

            var interfaceGraphTypeFieldRecords = fieldStore.GetInterfaceGraphTypes();
            if (intGraphTypeFieldRecords != null)
            {
                foreach (var interfaceTypeItem in interfaceGraphTypeFieldRecords)
                {
                    FieldRecord<InterfaceGraphType> record = interfaceTypeItem;
                    Func<ResolveFieldContext<object>, Task<object>> stringTypeResolver =
                        record.Resolve;

                    FieldAsync<InterfaceGraphType>(name: record.Name,
                        description: record.Description,
                        arguments: record.QueryArguments,
                        resolve: stringTypeResolver,
                        deprecationReason: record.DeprecationReason);

                }
            }

            var listGraphTypeFieldRecords = fieldStore.GetListGraphTypes();
            if (listGraphTypeFieldRecords != null)
            {
                foreach (var listTypeItem in listGraphTypeFieldRecords)
                {
                    FieldRecord<ListGraphType> record = listTypeItem;
                    Func<ResolveFieldContext<object>, Task<object>> stringTypeResolver =
                        record.Resolve;

                    FieldAsync<ListGraphType>(name: record.Name,
                        description: record.Description,
                        arguments: record.QueryArguments,
                        resolve: stringTypeResolver,
                        deprecationReason: record.DeprecationReason);

                }
            }

            var unionGraphTypeFieldRecords = fieldStore.GetUnionGraphTypes();
            if (unionGraphTypeFieldRecords != null)
            {
                foreach (var unionTypeItem in unionGraphTypeFieldRecords)
                {
                    FieldRecord<UnionGraphType> record = unionTypeItem;
                    Func<ResolveFieldContext<object>, Task<object>> stringTypeResolver =
                        record.Resolve;

                    FieldAsync<UnionGraphType>(name: record.Name,
                        description: record.Description,
                        arguments: record.QueryArguments,
                        resolve: stringTypeResolver,
                        deprecationReason: record.DeprecationReason);

                }
            }
        }
    }
}

