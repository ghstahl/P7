using System;
using System.Threading.Tasks;
using P7.BlogStore.Core;

namespace P7.BlogStore.Hugo
{
    public class HugoBlogCommentStore : HugoStoreBase<BlogComment>, IBlogCommentStore
    {
        public HugoBlogCommentStore(IBiggyConfiguration biggyConfiguration) :
            base(biggyConfiguration, "blog-comment")
        {
        }

        public async Task InsertAsync(Guid blogId, BlogComment blogComment)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Guid blogId, BlogComment blogComment)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid blogId, Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<BlogComment> FetchAsync(Guid blogId, Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IPage<BlogComment>> PageAsync(Guid blogId, int pageSize, byte[] pagingState)
        {
            throw new NotImplementedException();
        }
    }
}