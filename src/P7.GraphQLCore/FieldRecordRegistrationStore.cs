using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;

namespace P7.GraphQLCore
{

    public class MutationFieldRecordRegistrationStore :
        IMutationFieldRecordRegistrationStore
    {
        private IEnumerable<IMutationFieldRecordRegistration> _fieldRecordRegistrations;

        public MutationFieldRecordRegistrationStore(
            IEnumerable<IMutationFieldRecordRegistration> fieldRecordRegistrations)
        {
            _fieldRecordRegistrations = fieldRecordRegistrations;
        }

        public void AddGraphTypeFields(MutationCore mutationCore)
        {
            foreach (var item in _fieldRecordRegistrations)
            {
                item.AddGraphTypeFields(mutationCore);
            }
        }
    }

    public class QueryFieldRecordRegistrationStore : IQueryFieldRecordRegistrationStore
    {
        private IEnumerable<IQueryFieldRecordRegistration> _fieldRecordRegistrations;

        public QueryFieldRecordRegistrationStore(IEnumerable<IQueryFieldRecordRegistration> fieldRecordRegistrations)
        {
            _fieldRecordRegistrations = fieldRecordRegistrations;
        }

        public void AddGraphTypeFields(QueryCore queryCore)
        {
            foreach (var item in _fieldRecordRegistrations)
            {
                item.AddGraphTypeFields(queryCore);
            }
        }
    }
}
