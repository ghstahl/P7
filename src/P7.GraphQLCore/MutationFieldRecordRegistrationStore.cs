using System.Collections.Generic;

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
}