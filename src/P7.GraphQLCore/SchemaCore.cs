using System;
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
}