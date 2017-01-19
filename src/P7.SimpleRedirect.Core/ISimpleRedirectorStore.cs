using System.Threading.Tasks;

namespace P7.SimpleRedirect.Core
{
    public class SimpleRedirectRecord
    {
        public string Key { get; set; }
        public string BaseUrl { get; set; }
        public string Scheme { get; set; }
    }
    public interface ISimpleRedirectorStore
    {
        /// <summary>
        /// Finds the redirector record by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns>null or RedirectRecord</returns>
        Task<SimpleRedirectRecord> FetchRedirectRecord(string key);
    }
}
