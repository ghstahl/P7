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
    public class MyQueryFieldRecordRegistrationBase : QueryFieldRecordRegistrationBase
    {
        private IResourceFetcher _resourceFetcher;

        public MyQueryFieldRecordRegistrationBase(IResourceFetcher resourceFetcher)
        {
            _resourceFetcher = resourceFetcher;
        }

        private List<FieldRecord<StringGraphType>> _listStringGraphTypeFieldRecords;

        private List<FieldRecord<StringGraphType>> ListStringGraphTypeFieldRecords
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

        public  void PopulateStringGraphTypes( )
        {
            ListStringGraphTypeFieldRecords.Add(new FieldRecord<StringGraphType>()
            {
                Name = "resource",
                QueryArguments = new QueryArguments(new QueryArgument<ResourceQueryInput> { Name = "input" }),

                Resolve = async context =>
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
                        var obj = await _resourceFetcher.GetResourceSetAsync(
                            new ResourceQueryHandle()
                            {
                                Culture = currentCulture.Name,
                                Id = input.Id,
                                Treatment = input.Treatment
                            });
                        return obj;
                }
            });
        }
        public override IEnumerable<FieldRecord<StringGraphType>> GetStringGraphTypes()
        {
            return ListStringGraphTypeFieldRecords;
        }
    }
}
