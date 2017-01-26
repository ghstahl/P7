using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using P7.Core.Localization;
using P7.Core.Localization.Treatment;
using P7.Core.Reflection;
using P7.GraphQLCore;

namespace P7.Globalization
{
    public class MyFieldRecordRegistration: IQueryFieldRecordRegistration
    {
        private IStringLocalizerFactory _localizerFactory;
        private IMemoryCache _cache;
        private ITreatmentMap _treatmentMap;
        public MyFieldRecordRegistration(
                IStringLocalizerFactory localizerFactory,
               IMemoryCache cache,
               ITreatmentMap treatmentMap)
        {
            _localizerFactory = localizerFactory;
            _cache = cache;
            _treatmentMap = treatmentMap;
        }
        private object InternalGetResourceSet(string id, string treatment, CultureInfo cultureInfo)
        {
            try
            {
                var typeId = TypeHelper<Type>.GetTypeByFullName(id);
                if (typeId != null)
                {
                    if (string.IsNullOrEmpty(treatment))
                    {
                        treatment = "kvo";
                    }
                    var treatmentObject = _treatmentMap.GetTreatment(treatment);
                    if (treatmentObject == null)
                    {
                        treatment = "kvo";
                        treatmentObject = _treatmentMap.GetTreatment(treatment);
                    }
                   
                    var localizer = _localizerFactory.Create(typeId);

                    var resourceSet = localizer.WithCulture(cultureInfo).GetAllStrings(true);
                    var result = treatmentObject.Process(resourceSet);
                    return result;
                }
            }
            catch (Exception e)
            {
                return "";
            }
            return "";
        }

        private object GetResourceSet(ResourceQueryHandle input)
        {
            CultureInfo currentCulture = new CultureInfo(input.Culture);
            var key = new List<object> { currentCulture, input.Id, input.Treatment }
                              .AsReadOnly().GetSequenceHashCode();
            var newValue = new Lazy<object>(() =>
            {
                return InternalGetResourceSet(input.Id, input.Treatment, currentCulture);
            });
            var value = _cache.GetOrCreate(key.ToString(CultureInfo.InvariantCulture), entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(100);
                return newValue;
            });

            var result = value != null ? value.Value : newValue.Value;
            return result;
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
                                        
                                    }
                                }
                                var key = new List<object> {currentCulture, input.Id, input.Treatment}
                                    .AsReadOnly().GetSequenceHashCode();
                                var newValue = new Lazy<object>(() =>
                                {
                                    return InternalGetResourceSet(input.Id, input.Treatment, currentCulture);

                                });


                                var value = _cache.GetOrCreate(key.ToString(CultureInfo.InvariantCulture), entry =>
                                {
                                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(100);
                                    return newValue;
                                });

                                var resultValue = value != null ? value.Value : newValue.Value;
                                return resultValue;

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
