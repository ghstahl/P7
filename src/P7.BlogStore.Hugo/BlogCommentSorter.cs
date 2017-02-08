using System.Collections.Generic;
using System.Linq;
using Hugo.Data.Json;
using P7.BlogStore.Core;

namespace P7.BlogStore.Hugo
{
    public class BlogCommentSorter : ISorter<BlogComment>
    {
        public List<BlogComment> Sort(List<BlogComment> items)
        {
            return items.OrderBy(o => o.TimeStamp).ToList();
        }
    }
}