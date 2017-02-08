using System.Collections.Generic;
using System.Linq;
using Hugo.Data.Json;
using P7.BlogStore.Core;

namespace P7.BlogStore.Hugo
{
    public class BlogSorter : ISorter<Blog>
    {
        public List<Blog> Sort(List<Blog> items)
        {
            return items.OrderBy(o => o.TimeStamp).ToList();

        }
    }
}