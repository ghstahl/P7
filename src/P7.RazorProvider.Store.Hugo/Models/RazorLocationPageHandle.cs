using System;
using System.Collections.Generic;

namespace P7.RazorProvider.Store.Hugo.Models
{
    public class RazorLocationPageHandle
    {
        public int PageSize { get; set; }
        public string PagingState { get; set; }
        public DateTime TimeStampLowerBoundary { get; set; }
        public DateTime TimeStampUpperBoundary { get; set; }

        public RazorLocationPageHandle()
        {
        }

        public RazorLocationPageHandle(RazorLocationPageHandle doc)
        {
            this.PageSize = doc.PageSize;
            this.PagingState = doc.PagingState;
            this.TimeStampLowerBoundary = doc.TimeStampLowerBoundary;
            this.TimeStampUpperBoundary = doc.TimeStampUpperBoundary;
        }
    }
}