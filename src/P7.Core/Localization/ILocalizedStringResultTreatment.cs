using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace P7.Core.Localization
{
    public interface ILocalizedStringResultTreatment
    {
        object Process(IEnumerable<LocalizedString> resourceSet);
    }
}
