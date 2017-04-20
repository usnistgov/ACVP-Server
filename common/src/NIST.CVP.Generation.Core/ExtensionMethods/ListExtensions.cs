using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.Generation.Core.ExtensionMethods
{
    /// <summary>
    /// Extension methods for <see cref="List{T}"/>
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Adds a string to a <see cref="List{T}"/> when the string is not null or empty, otherwise do nothing.
        /// </summary>
        /// <param name="list">The list to (potentially) add the <see cref="itemToAdd"/> too</param>
        /// <param name="itemToAdd">The item that is evaluated for adding to <see cref="list"/></param>
        public static void AddIfNotNullOrEmpty(this List<string> list, string itemToAdd)
        {
            if (!string.IsNullOrEmpty(itemToAdd))
            {
                list.Add(itemToAdd);
            }
        }

        /// <summary>
        /// Adds an <see cref="IEnumerable{T}"/> to a <see cref="List{T}"/> when the <see cref="itemsToAdd"/> has more than 0 elements.
        /// </summary>
        /// <param name="list">The list to (potentially) add the <see cref="itemsToAdd"/> too</param>
        /// <param name="itemsToAdd">The object that is evaluated for adding to <see cref="list"/></param>
        public static void AddRangeIfNotNullOrEmpty<T>(this List<T> list, IEnumerable<T> itemsToAdd)
        {
            if (itemsToAdd != null && itemsToAdd.Any())
            {
                list.AddRange(itemsToAdd);
            }
        }
    }
}
