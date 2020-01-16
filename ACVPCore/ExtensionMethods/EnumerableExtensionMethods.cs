using System.Collections.Generic;
using ACVPCore.Models;

namespace ACVPCore.ExtensionMethods
{
    public static class EnumerableExtensionMethods
    {
        /// <summary>
        /// Wraps an <see cref="IEnumerable{T}"/> for use when controller action returns an array of object.
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
    }
}