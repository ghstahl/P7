using GraphQL.Types;

namespace P7.GraphQLCore
{
    public class MutationCore : ObjectGraphType<object>
    {
        public MutationCore(IMutationFieldRecordRegistrationStore fieldStore)
        {
            Name = "Mutation";
            fieldStore.AddGraphTypeFields(this);
        }
    }
}