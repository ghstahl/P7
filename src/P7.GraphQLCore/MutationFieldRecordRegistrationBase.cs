using System.Collections.Generic;
using GraphQL.Types;

namespace P7.GraphQLCore
{
    public abstract class MutationFieldRecordRegistrationBase : IMutationFieldRecordRegistration
    {
        public virtual IEnumerable<FieldRecord<StringGraphType>> GetStringGraphTypes()
        {
            return null;
        }
        public virtual IEnumerable<FieldRecord<BooleanGraphType>> GetBooleanGraphTypes()
        {
            return null;
        }

        public virtual IEnumerable<FieldRecord<DateGraphType>> GetDateGraphTypes()
        {
            return null;
        }

        public virtual IEnumerable<FieldRecord<DecimalGraphType>> GetDecimalGraphTypes()
        {
            return null;
        }

        public virtual IEnumerable<FieldRecord<EnumerationGraphType>> GetEnumerationGraphTypes()
        {
            return null;
        }

        public virtual IEnumerable<FieldRecord<FloatGraphType>> GetFloatGraphTypes()
        {
            return null;
        }

        public virtual IEnumerable<FieldRecord<IntGraphType>> GetIntGraphTypeTypes()
        {
            return null;
        }

        public virtual IEnumerable<FieldRecord<InterfaceGraphType>> GetInterfaceGraphTypes()
        {
            return null;
        }

        public virtual IEnumerable<FieldRecord<ListGraphType>> GetListGraphTypes()
        {
            return null;
        }

        public virtual IEnumerable<FieldRecord<UnionGraphType>> GetUnionGraphTypes()
        {
            return null;
        }
    }
}