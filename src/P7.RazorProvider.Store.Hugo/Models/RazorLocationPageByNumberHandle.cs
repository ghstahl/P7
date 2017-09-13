using System;

namespace P7.RazorProvider.Store.Hugo.Models
{
    public class RazorLocationPageByNumberHandle
    {
        public int PageSize { get; set; }
        public int Page { get; set; }
        public DateTime TimeStampLowerBoundary { get; set; }
        public DateTime TimeStampUpperBoundary { get; set; }
        public RazorLocationPageByNumberHandle()
        {
        }

        public RazorLocationPageByNumberHandle(RazorLocationPageByNumberHandle doc)
        {
            this.PageSize = doc.PageSize;
            this.Page = doc.Page;
            this.TimeStampLowerBoundary = doc.TimeStampLowerBoundary;
            this.TimeStampUpperBoundary = doc.TimeStampUpperBoundary;
        }
    }
}