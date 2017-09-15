using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using P7.Core.Linq;
using P7.RazorProvider.Store.Hugo.Interfaces;
using P7.RazorProvider.Store.Hugo.Models;
using P7.SimpleDocument.Store;
using P7.SimpleDocument.Store.Hugo;
using P7.Store;

namespace P7.RazorProvider.Store.Hugo
{
    public class HugoRazorLocationStore : HugoSimpleDocumentStoreTenantAware<RazorLocation>, IRazorLocationStore
    {

        public HugoRazorLocationStore(IRazorLocationStoreBiggyConfiguration biggyConfiguration)
            : base( biggyConfiguration, "razor-location")
        {
        }
        public async Task<IPage<SimpleDocument<RazorLocation>>> PageAsync(int pageSize, byte[] pagingState,
            DateTime? timeStampLowerBoundary = null,
            DateTime? timeStampUpperBoundary = null)
        {
            byte[] currentPagingState = pagingState;
            PagingState ps = pagingState.DeserializePageState();
            var records = await RetrieveAsync();
            records = records.OrderBy(o => o.Document.LastModified).ToList();

            var predicate = PredicateBuilder.True<SimpleDocument<RazorLocation>>();
            if (timeStampLowerBoundary != null)
            {
                predicate = predicate.And(i => i.Document.LastModified >= timeStampLowerBoundary);
            }
            if (timeStampUpperBoundary != null)
            {
                predicate = predicate.And(i => i.Document.LastModified <= timeStampUpperBoundary);
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

            var page = new PageProxy<SimpleDocument<RazorLocation>>(currentPagingState, pagingState, slice);
            return page;
        }

        public Task<IPage<SimpleDocument<RazorLocation>>> PageAsync(int pageSize, int page, DateTime? timeStampLowerBoundary = null, DateTime? timeStampUpperBoundary = null,
            string[] categories = null, string[] tags = null)
        {
            throw new NotImplementedException();
        }

        public async Task<IPage<SimpleDocument<RazorLocation>>> PageAsync(int pageSize, int page, 
            DateTime? timeStampLowerBoundary = default(DateTime?),
            DateTime? timeStampUpperBoundary = default(DateTime?))
        {
            PagingState ps = new PagingState() { CurrentIndex = pageSize * (page - 1) };
            var pagingState = ps.Serialize();

            return await PageAsync(pageSize, pagingState, timeStampLowerBoundary, timeStampUpperBoundary);
        }

    }
}
