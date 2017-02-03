using System.Collections.Generic;
using System.Linq;
using GraphQL.Types;

namespace P7.GraphQLCore
{
    public interface IQueryFieldRecordRegistrationStore
    {
        void AddGraphTypeFields(QueryCore queryCore);
    }

    public interface IMutationFieldRecordRegistrationStore
    {
        void AddGraphTypeFields(MutationCore mutationCore);
    }
}

