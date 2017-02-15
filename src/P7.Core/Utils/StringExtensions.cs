using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P7.Core.Utils
{
    public static class StringExtensions
    {
        public static Guid AsGuid(this string value)
        {
            return Guid.Parse(value);
        }
    }
}
