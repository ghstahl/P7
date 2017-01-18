using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace P7.Core.Localization.Treatment
{
    public class KeyValueObject: ILocalizedStringResultTreatment
    {
        public object Process(IEnumerable<LocalizedString> resourceSet)
        {
            var expando = new System.Dynamic.ExpandoObject();
            var expandoMap = expando as IDictionary<string, object>;
            foreach (var rs in resourceSet)
            {
                expandoMap[rs.Name] = rs.Value;
            }
            return expando;
        }
    }
}
