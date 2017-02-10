using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hugo.Data.Json;
using P7.BlogStore.Core;
using P7.Core.Linq;
using P7.HugoStore.Core;
using P7.Store;

namespace P7.BlogStore.Hugo
{
    public class HugoBlogStore: HugoStoreBase<Blog>, IBlogStore
    {
        public delegate bool ContainsAnyInTagsOrCategories(Blog source, List<string> tags, List<string> categories);

        private ContainsAnyInTagsOrCategories _containsAnyInTagsOrCategories;

        public ContainsAnyInTagsOrCategories DelegateContainsAnyInTagsOrCategories
        {
            get
            {
                if (_containsAnyInTagsOrCategories == null)
                {
                    _containsAnyInTagsOrCategories = (blog, tags,categories) =>
                    {
                        if (tags == null && categories == null)
                            return true;
                        var result = false;
                        if (tags != null)
                        {
                            var bTags = blog.Tags.Any(x => tags.Contains(x));
                            result = result || bTags;
                        }
                        if (categories != null)
                        {
                            var bCategories = blog.Categories.Any(x => categories.Contains(x));
                            result = result || bCategories;
                        }
                        return result;
                    };
                }
                return _containsAnyInTagsOrCategories;
            }
        }
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
            records =  records.OrderBy(o => o.TimeStamp).ToList();

            var predicate = PredicateBuilder.True<Blog>();
            if (timeStampLowerBoundary != null)
            {
                predicate = predicate.And(i => i.TimeStamp >= timeStampLowerBoundary);
            }
            if (timeStampUpperBoundary != null)
            {
                predicate = predicate.And(i => i.TimeStamp <= timeStampUpperBoundary);
            }

            // this is an AND that return an OR match for tags and categories.
            List<string> safeTagList = (tags == null) ? null : new List<string>(tags);
            List<string> safeCategoriesList = (categories == null) ? null : new List<string>(categories);
            predicate = predicate.And(i => DelegateContainsAnyInTagsOrCategories(i, safeTagList, safeCategoriesList));


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
