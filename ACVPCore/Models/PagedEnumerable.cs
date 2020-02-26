using System;
using System.Collections.Generic;

namespace ACVPCore.Models
{
    public class PagedEnumerable<T> : WrappedEnumerable<T>
    {
        public int PageSize { get; }
        public int CurrentPage { get; }
        public long TotalRecords { get; }
        
        public PagedEnumerable(IEnumerable<T> data, int pageSize, int currentPage, long totalRecords) : base(data)
        {
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalRecords = totalRecords;
        }
    }
}