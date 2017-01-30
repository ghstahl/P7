using System.Collections.Generic;
using GraphQL.Types;

namespace P7.GraphQLCore
{
    public abstract class MutationFieldRecordRegistrationBase : IMutationFieldRecordRegistration
    {
        private List<FieldRecord<StringGraphType>> _listStringGraphTypeFieldRecords;

        protected List<FieldRecord<StringGraphType>> ListStringGraphTypeFieldRecords
        {
            get
            {
                if (_listStringGraphTypeFieldRecords == null)
                {
                    _listStringGraphTypeFieldRecords = new List<FieldRecord<StringGraphType>>();
                    PopulateStringGraphTypes();
                }
                return _listStringGraphTypeFieldRecords;
            }
        }

        protected virtual void PopulateStringGraphTypes()
        {
        }
        public IEnumerable<FieldRecord<StringGraphType>> GetStringGraphTypes()
        {
            return ListStringGraphTypeFieldRecords;
        }
        public IEnumerable<FieldRecord<BooleanGraphType>> GetBooleanGraphTypes()
        {
            return null;
        }

        public IEnumerable<FieldRecord<DateGraphType>> GetDateGraphTypes()
        {
            return null;
        }

        public IEnumerable<FieldRecord<DecimalGraphType>> GetDecimalGraphTypes()
        {
            return null;
        }

        public IEnumerable<FieldRecord<EnumerationGraphType>> GetEnumerationGraphTypes()
        {
            return null;
        }

        public IEnumerable<FieldRecord<FloatGraphType>> GetFloatGraphTypes()
        {
            return null;
        }

        public IEnumerable<FieldRecord<IntGraphType>> GetIntGraphTypeTypes()
        {
            return null;
        }

        public IEnumerable<FieldRecord<InterfaceGraphType>> GetInterfaceGraphTypes()
        {
            return null;
        }

        public IEnumerable<FieldRecord<ListGraphType>> GetListGraphTypes()
        {
            return null;
        }

        public IEnumerable<FieldRecord<UnionGraphType>> GetUnionGraphTypes()
        {
            return null;
        }
    }
}