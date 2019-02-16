using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NIST.CVP.Common.ExtensionMethods
{
    /// <summary>
    /// Extension methods for <see cref="IEnumerable{T}"/>
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// returns first item found, or null if not found.
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="items">The IEnumerable to iterate</param>
        /// <param name="predicate">What to search for</param>
        /// <returns></returns>
        public static T? FirstOrNull<T>(this IEnumerable<T> items, Func<T, bool> predicate) where T : struct
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            foreach (var item in items)
            {
                if (predicate(item))
                    return item;
            }
            return null;
        }

        /// <summary>
        /// Try to get an item from the <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="items">The IEnumerable to iterate</param>
        /// <param name="predicate"></param>
        /// <param name="result">The type to return when found</param>
        /// <returns></returns>
        public static bool TryFirst<T>(this IEnumerable<T> items, Func<T, bool> predicate, out T result)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            result = default(T);
            foreach (var item in items)
            {
                if (predicate(item))
                {
                    result = item;
                    return true;
                }
            }
            return false;
        }
    }
}
