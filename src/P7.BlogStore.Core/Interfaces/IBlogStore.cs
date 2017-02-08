using System;
using System.Threading.Tasks;
using P7.Store;

namespace P7.BlogStore.Core
{
    public interface IBlogStore
    {
        #region Blog

        /// <summary>
        /// Creates a new Blog
        /// </summary>
        /// <param name="blog"></param>
        /// <returns></returns>
        Task InsertAsync(Blog blog);

        /// <summary>
        /// Updates a new Blog
        /// </summary>
        /// <param name="blog"></param>
        /// <returns></returns>
        Task UpdateAsync(Blog blog);


        /// <summary>
        /// Delete the blog
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Finds the blog
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Blog> FetchAsync(Guid id);

        /// <summary>
        /// Pages all documents
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pagingState"></param>
        /// <param name="categories"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        Task<IPage<Blog>> PageAsync(int pageSize,
            byte[] pagingState,
            DateTime? timeStampLowerBoundary = null,
            DateTime? timeStampUpperBoundary = null,
            string[] categories = null,
            string[] tags = null);

        #endregion
    }
}
