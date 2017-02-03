using System;
using System.Linq;
using System.Threading.Tasks;
using P7.BlogStore.Core;

using P7.Store;

namespace P7.BlogStore.Hugo
{
    public class HugoBlogCommentStore : HugoStoreBase<BlogCommentRecord>, IBlogCommentStore
    {
        public HugoBlogCommentStore(IBiggyConfiguration biggyConfiguration) :
            base(biggyConfiguration, "blog-comment",null)
        {
        }

        public async Task InsertAsync(Guid blogId, BlogComment blogComment)
        {
            BlogCommentRecord record = new BlogCommentRecord() {BlogId = blogId.ToString(), BlogComment = blogComment};
            await InsertAsync(record);
        }

        public async Task UpdateAsync(Guid blogId, BlogComment blogComment)
        {
            BlogCommentRecord record = new BlogCommentRecord() { BlogId = blogId.ToString(), BlogComment = blogComment };
            await UpdateAsync(record);
        }

        public async Task DeleteAsync(Guid blogId, Guid id)
        {
            BlogCommentRecord record = new BlogCommentRecord()
            {
                BlogId = blogId.ToString(), BlogComment = new BlogComment() {Id = id.ToString()}
            };
            await DeleteAsync(record.Id_G);
        }

        public async Task<BlogComment> FetchAsync(Guid blogId, Guid id)
        {
            BlogCommentRecord record = new BlogCommentRecord() { BlogId = blogId.ToString(), BlogComment = new BlogComment() { Id = id.ToString() } };
            var result = await FetchAsync(record.Id_G);
            return result.BlogComment;
        }

        public async Task<IPage<BlogComment>> PageAsync(Guid blogId, int pageSize, byte[] pagingState)
        {
            byte[] currentPagingState = pagingState;
            PagingState ps = pagingState.Deserialize();
            var records = await RetrieveAsync();

            // only interested in this blog's comments, not all of them.
            var query = from item in records
                where item.BlogId_G == blogId
                select item.BlogComment;

            var slice = query.Skip(ps.CurrentIndex).Take(pageSize).ToList();
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

            var page = new PageProxy<BlogComment>(currentPagingState, pagingState, slice);
            return page;
        }
    }
}