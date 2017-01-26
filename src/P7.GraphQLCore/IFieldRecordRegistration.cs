using System.Collections.Generic;
using GraphQL.Types;

namespace P7.GraphQLCore
{
    public interface IQueryFieldRecordRegistration: IFieldRecordRegistration
    {
    }

    public interface IFieldRecordRegistration
    {
        IEnumerable<FieldRecord<StringGraphType>> GetStringGraphTypes();
        IEnumerable<FieldRecord<BooleanGraphType>> GetBooleanGraphTypes();
        IEnumerable<FieldRecord<DateGraphType>> GetDateGraphTypes();
        IEnumerable<FieldRecord<DecimalGraphType>> GetDecimalGraphTypes();
        IEnumerable<FieldRecord<EnumerationGraphType>> GetEnumerationGraphTypes();
        IEnumerable<FieldRecord<FloatGraphType>> GetFloatGraphTypes();
        IEnumerable<FieldRecord<IntGraphType>> GetIntGraphTypeTypes();
        IEnumerable<FieldRecord<InterfaceGraphType>> GetInterfaceGraphTypes();
        IEnumerable<FieldRecord<ListGraphType>> GetListGraphTypes();
        IEnumerable<FieldRecord<UnionGraphType>> GetUnionGraphTypes();
    }
}