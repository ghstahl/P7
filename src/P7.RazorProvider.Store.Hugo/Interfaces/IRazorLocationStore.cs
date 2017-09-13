using System;
using System.Threading.Tasks;
using P7.RazorProvider.Store.Hugo.Models;
using P7.SimpleDocument.Store;
using P7.Store;

namespace P7.RazorProvider.Store.Hugo.Interfaces
{
    public interface IRazorLocationStore : ISimpleDocumentStore<RazorLocation>
    {
        #region RazorLocation

        /// <summary>
        /// Pages all documents
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pagingState"></param>
        /// <param name="categories"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        Task<IPage<SimpleDocument<RazorLocation>>> PageAsync(int pageSize,
            byte[] pagingState,
            DateTime? timeStampLowerBoundary = null,
            DateTime? timeStampUpperBoundary = null);

        /// <summary>
        /// Pages all documents
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <param name="categories"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        Task<IPage<SimpleDocument<RazorLocation>>> PageAsync(int pageSize,
            int page,
            DateTime? timeStampLowerBoundary = null,
            DateTime? timeStampUpperBoundary = null,
            string[] categories = null,
            string[] tags = null);
        
        #endregion

        /// <summary>
        /// Gets the tenantId of this store.
        /// </summary>
        /// <returns></returns>
        Task<string> GetTenantIdAsync();
    }
}
