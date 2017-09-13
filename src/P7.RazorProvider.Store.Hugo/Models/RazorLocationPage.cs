using System.Collections.Generic;
using P7.SimpleDocument.Store;

namespace P7.RazorProvider.Store.Hugo.Models
{
    public class RazorLocationPage
    {
        public string CurrentPagingState { get; set; }
        public string PagingState { get; set; }
        public List<SimpleDocument<RazorLocation>> RazorLocations { get; set; }
    }
}