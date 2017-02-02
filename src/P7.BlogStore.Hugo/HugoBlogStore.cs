using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hugo.Data.Json;
using P7.BlogStore.Core;
using P7.Core.Linq;
using P7.Store;

namespace P7.BlogStore.Hugo
{
    public class HugoBlogStore: HugoStoreBase<Blog>, IBlogStore
    {
        public HugoBlogStore(IBiggyConfiguration biggyConfiguration,
            ISorter<Blog> sorter) :
            base(biggyConfiguration,"blog", sorter)
        {

        }

        public async Task<IPage<Blog>> PageAsync(int pageSize, byte[] pagingState, DateTime? timeStampLowerBoundary = null,
            DateTime? timeStampUpperBoundary = null, string[] categories = null, string[] tags = null)
        {
            byte[] currentPagingState = pagingState;
            PagingState ps = pagingState.Deserialize();
            var records = await RetrieveAsync();
            var predicate = PredicateBuilder.True<Blog>();
            if (timeStampLowerBoundary != null)
            {
                predicate = predicate.And(i => i.TimeStamp >= timeStampLowerBoundary);
            }
            if (timeStampUpperBoundary != null)
            {
                predicate = predicate.And(i => i.TimeStamp <= timeStampUpperBoundary);
            }

            if (categories != null && categories.Length > 0)
            {
                predicate = predicate.And(i => DelegateContainsAnyInStringList(i.Categories, new List<string>(categories)));
            }
            if (tags != null && tags.Length > 0)
            {
                predicate = predicate.And(i => DelegateContainsAnyInStringList(i.Tags, new List<string>(tags)));
            }

            var filtered = records.Where(predicate.Compile()).Select(i => i);

            var slice = filtered.Skip(ps.CurrentIndex).Take(pageSize).ToList();
            if (slice.Count < pageSize)
            {
                // we are at the end
                pagingState = null;
            }
            else
            {
                ps.CurrentIndex += pageSize;
                pagingState = ps.Serialize();
            }

            var page = new PageProxy<Blog>(currentPagingState, pagingState, slice);
            return page;
        }
    }
}
