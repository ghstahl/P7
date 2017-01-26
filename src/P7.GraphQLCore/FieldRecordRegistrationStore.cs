using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;

namespace P7.GraphQLCore
{

    public class QueryFieldRecordRegistrationStore : FieldRecordRegistrationStore, IQueryFieldRecordRegistrationStore
    {
        public QueryFieldRecordRegistrationStore(IEnumerable<IQueryFieldRecordRegistration> fieldRecordRegistrations)
        {
            FieldRecordRegistrations = from item in fieldRecordRegistrations
                let c = item as IFieldRecordRegistration
                select c;
        }
    }

    public abstract class FieldRecordRegistrationStore : IFieldRecordRegistrationStore
    {
        public FieldRecordRegistrationStore() { }
        protected IEnumerable<IFieldRecordRegistration> FieldRecordRegistrations { get; set; }

        private List<FieldRecord<StringGraphType>> _getStringGraphTypes;
        public IEnumerable<FieldRecord<StringGraphType>> GetStringGraphTypes()
        {
            if (_getStringGraphTypes == null)
            {
                var query = from item in FieldRecordRegistrations
                            let c = item.GetStringGraphTypes()
                            where c != null
                            select c;
                _getStringGraphTypes = new List<FieldRecord<StringGraphType>>();
                foreach (var q in query)
                {
                    _getStringGraphTypes.AddRange(q);
                }
            }
            return _getStringGraphTypes;
            
        }

        private List<FieldRecord<BooleanGraphType>> _getBooleanGraphTypes;
        public IEnumerable<FieldRecord<BooleanGraphType>> GetBooleanGraphTypes()
        {
            if (_getBooleanGraphTypes == null)
            {
                var query = from item in FieldRecordRegistrations
                            let c = item.GetBooleanGraphTypes()
                            where c != null
                            select c;
                _getBooleanGraphTypes = new List<FieldRecord<BooleanGraphType>>();
                foreach (var q in query)
                {
                    _getBooleanGraphTypes.AddRange(q);
                }
            }
            return _getBooleanGraphTypes;
        }

        private List<FieldRecord<DateGraphType>> _getDateGraphTypes;
        public IEnumerable<FieldRecord<DateGraphType>> GetDateGraphTypes()
        {
            if (_getDateGraphTypes == null)
            {
                var query = from item in FieldRecordRegistrations
                            let c = item.GetDateGraphTypes()
                            where c != null
                            select c;
                _getDateGraphTypes = new List<FieldRecord<DateGraphType>>();
                foreach (var q in query)
                {
                    _getDateGraphTypes.AddRange(q);
                }
            }
            return _getDateGraphTypes;
        }

        private List<FieldRecord<DecimalGraphType>> _getDecimalGraphTypes;
        public IEnumerable<FieldRecord<DecimalGraphType>> GetDecimalGraphTypes()
        {
            if (_getDecimalGraphTypes == null)
            {
                var query = from item in FieldRecordRegistrations
                            let c = item.GetDecimalGraphTypes()
                            where c != null
                            select c;
                _getDecimalGraphTypes = new List<FieldRecord<DecimalGraphType>>();
                foreach (var q in query)
                {
                    _getDecimalGraphTypes.AddRange(q);
                }
            }
            return _getDecimalGraphTypes;
        }

        private List<FieldRecord<EnumerationGraphType>> _getEnumerationGraphTypes;
        public IEnumerable<FieldRecord<EnumerationGraphType>> GetEnumerationGraphTypes()
        {
            if (_getEnumerationGraphTypes == null)
            {
                var query = from item in FieldRecordRegistrations
                            let c = item.GetEnumerationGraphTypes()
                            where c != null
                            select c;
                _getEnumerationGraphTypes = new List<FieldRecord<EnumerationGraphType>>();
                foreach (var q in query)
                {
                    _getEnumerationGraphTypes.AddRange(q);
                }
            }
            return _getEnumerationGraphTypes;
        }

        private List<FieldRecord<FloatGraphType>> _getFloatGraphTypes;
        public IEnumerable<FieldRecord<FloatGraphType>> GetFloatGraphTypes()
        {
            if (_getFloatGraphTypes == null)
            {
                var query = from item in FieldRecordRegistrations
                            let c = item.GetFloatGraphTypes()
                            where c != null
                            select c;
                _getFloatGraphTypes = new List<FieldRecord<FloatGraphType>>();
                foreach (var q in query)
                {
                    _getFloatGraphTypes.AddRange(q);
                }
            }
            return _getFloatGraphTypes;
        }

        private List<FieldRecord<IntGraphType>> _getIntGraphTypes;
        public IEnumerable<FieldRecord<IntGraphType>> GetIntGraphTypeTypes()
        {
            if (_getIntGraphTypes == null)
            {
                var query = from item in FieldRecordRegistrations
                            let c = item.GetIntGraphTypeTypes()
                            where c != null
                            select c;
                _getIntGraphTypes = new List<FieldRecord<IntGraphType>>();
                foreach (var q in query)
                {
                    _getIntGraphTypes.AddRange(q);
                }
            }
            return _getIntGraphTypes;
        }

        private List<FieldRecord<InterfaceGraphType>> _getInterfaceGraphTypes;
        public IEnumerable<FieldRecord<InterfaceGraphType>> GetInterfaceGraphTypes()
        {
            if (_getInterfaceGraphTypes == null)
            {
                var query = from item in FieldRecordRegistrations
                            let c = item.GetInterfaceGraphTypes()
                            where c != null
                            select c;
                _getInterfaceGraphTypes = new List<FieldRecord<InterfaceGraphType>>();
                foreach (var q in query)
                {
                    _getInterfaceGraphTypes.AddRange(q);
                }
            }
            return _getInterfaceGraphTypes;
        }

        private List<FieldRecord<ListGraphType>> _getListGraphTypes;
        public IEnumerable<FieldRecord<ListGraphType>> GetListGraphTypes()
        {
            if (_getListGraphTypes == null)
            {
                var query = from item in FieldRecordRegistrations
                            let c = item.GetListGraphTypes()
                            where c != null
                            select c;
                _getListGraphTypes = new List<FieldRecord<ListGraphType>>();
                foreach (var q in query)
                {
                    _getListGraphTypes.AddRange(q);
                }
            }
            return _getListGraphTypes;
        }

        private List<FieldRecord<UnionGraphType>> _getUnionGraphTypes;
        public IEnumerable<FieldRecord<UnionGraphType>> GetUnionGraphTypes()
        {
            if (_getUnionGraphTypes == null)
            {
                var query = from item in FieldRecordRegistrations
                            let c = item.GetUnionGraphTypes()
                            where c != null
                            select c;
                _getUnionGraphTypes = new List<FieldRecord<UnionGraphType>>();
                foreach (var q in query)
                {
                    _getUnionGraphTypes.AddRange(q);
                }
            }
            return _getUnionGraphTypes;
        }
    }
}
