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
            Query = (QueryCore) resolveType(typeof(QueryCore));
            Mutation = (MutationCore) resolveType(typeof(MutationCore));
        }
    }

    public class MutationCore : ObjectGraphType<object>
    {
        public MutationCore(IMutationFieldRecordRegistrationStore fieldStore)
        {
            Name = "Mutation";
            fieldStore.AddGraphTypeFields(this);
        }
    }

    public class QueryCore : ObjectGraphType<object>
    {
        public QueryCore(IQueryFieldRecordRegistrationStore fieldStore)
        {
            Name = "Query";
            fieldStore.AddGraphTypeFields(this);
        }
    }
}

