using System.Collections.Generic;
using ACVPCore.Models;

namespace ACVPCore.ExtensionMethods
{
    public static class EnumerableExtensionMethods
    {
        /// <summary>
        /// Wraps an <see cref="IEnumerable{T}"/> for use when controller actions return an array of object.
        ///
        /// Wrapping the object is a result of https://haacked.com/archive/2008/11/20/anatomy-of-a-subtle-json-vulnerability.aspx/
        /// </summary>
        /// <param name="items">Items to wrap.</param>
        /// <typeparam name="T">The type of object wrapped.</typeparam>
        /// <returns><see cref="WrappedEnumerable{T}"/></returns>
        public static WrappedEnumerable<T> WrapEnumerable<T>(this IEnumerable<T> items)
        {
            return new WrappedEnumerable<T>(items);
        }

        /// <summary>
        /// Wraps an <see cref="IEnumerable{T}"/> with paging information for use when controller actions return an array of object.
        /// 
        /// </summary>
        /// <param name="items">Items to wrap.</param>
        /// <param name="pageSize">The page size (number of records to return).</param>
        /// <param name="currentPage">The current page of records.</param>
        /// <param name="totalRecords">The total number of records.</param>
        /// <typeparam name="T">The type of object wrapped.</typeparam>
        /// <returns><see cref="PagedEnumerable{T}"/></returns>
        public static PagedEnumerable<T> WrapPagingEnumerable<T>(this IEnumerable<T> items, int pageSize, int currentPage, long totalRecords)
        {
            return new PagedEnumerable<T>(items, pageSize, currentPage, totalRecords);
        }
    }
}