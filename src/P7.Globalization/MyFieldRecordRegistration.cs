using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;
using P7.Core.Localization;
using P7.GraphQLCore;

namespace P7.Globalization
{
    public class MyFieldRecordRegistration: IQueryFieldRecordRegistration
    {
        private IResourceFetcher _resourceFetcher;

        public MyFieldRecordRegistration(IResourceFetcher resourceFetcher)
        {
            _resourceFetcher = resourceFetcher;
        }

        private List<FieldRecord<StringGraphType>> _listStringGraphTypeFieldRecords;

        public IEnumerable<FieldRecord<StringGraphType>> GetStringGraphTypes()
        {
            if (_listStringGraphTypeFieldRecords == null)
            {
                _listStringGraphTypeFieldRecords = new List<FieldRecord<StringGraphType>>
                {
                    new FieldRecord<StringGraphType>()
                    {
                        Name = "resource",
                        QueryArguments = new QueryArguments(new QueryArgument<ResourceQueryInput> {Name = "input"}),

                        Resolve = async context =>
                        {
                            var result = await Task.Run(() =>
                            {

                                var input = context.GetArgument<ResourceQueryHandle>("input");
                                CultureInfo currentCulture = new CultureInfo("en-US");
                                if (!string.IsNullOrEmpty(input.Culture))
                                {
                                    try
                                    {
                                        currentCulture = new CultureInfo(input.Culture);
                                    }
                                    catch (Exception)
                                    {
                                        currentCulture = new CultureInfo("en-US");
                                    }
                                }
                                var obj = _resourceFetcher.GetResourceSet(
                                    new ResourceQueryHandle()
                                    {
                                        Culture = currentCulture.Name,
                                        Id = input.Id,
                                        Treatment = input.Treatment
                                    });
                                return obj;
                            });
                            return result;
                        }
                    }
                };
            }
            return _listStringGraphTypeFieldRecords;
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
