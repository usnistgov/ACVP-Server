using System.Collections.Generic;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Libraries.Shared.Enumerables
{
    public class PagedEnumerable<T> : WrappedEnumerable<T>
    {
        public int PageSize { get; }
        public int CurrentPage { get; }
        public long TotalRecords { get; }
        public long TotalPages => TotalRecords.CeilingDivide(PageSize);
        
        public PagedEnumerable(IEnumerable<T> data, int pageSize, int currentPage, long totalRecords) : base(data)
        {
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalRecords = totalRecords;
        }
    }
}