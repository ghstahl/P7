using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace P7.Core.Utils
{
    public static class NameValueCollectionExtensions
    {
        public static bool ContainsAny(this NameValueCollection nvc, IList<string> againstList)
        {
            if (againstList.Any(item => nvc[(string)item] == null))
            {
                return false;
            }
            return true;
        }
    }
}