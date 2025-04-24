using System;
using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods
{
    /// <summary>
    /// Extension methods for <see cref="List{T}"/>
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Adds a string to a <see cref="List{T}"/> when the string is not null or empty, otherwise do nothing.
        /// </summary>
        /// <param name="list">The list to (potentially) add the <see cref="itemToAdd"/> to</param>
        /// <param name="itemToAdd">The item that is evaluated for adding to <see cref="list"/></param>
        /// <returns>True if item added, false otherwise.</returns>
        public static bool AddIfNotNullOrEmpty(this List<string> list, string itemToAdd)
        {
            if (!string.IsNullOrEmpty(itemToAdd))
            {
                list.Add(itemToAdd);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Add an item to <see cref="List{T}"/> if it is not null
        /// </summary>
        /// <typeparam name="T">The type contained within the list</typeparam>
        /// <param name="list">The list</param>
        /// <param name="itemToAdd">The item to (potentially) add to the list</param>
        /// <returns>True if item added, false otherwise.</returns>
        public static bool AddIfNotNull<T>(this List<T> list, T itemToAdd)
            where T : class
        {
            if (itemToAdd == null)
            {
                return false;
            }

            list.Add(itemToAdd);
            return true;
        }

        /// <summary>
        /// Adds an <see cref="IEnumerable{T}"/> to a <see cref="List{T}"/> when the <see cref="itemsToAdd"/> has more than 0 elements.
        /// </summary>
        /// <param name="list">The list to (potentially) add the <see cref="itemsToAdd"/> too</param>
        /// <param name="itemsToAdd">The object that is evaluated for adding to <see cref="list"/></param>
        /// <returns>True if item added, false otherwise.</returns>
        public static bool AddRangeIfNotNullOrEmpty<T>(this List<T> list, IEnumerable<T> itemsToAdd)
        {
            if (itemsToAdd != null && itemsToAdd.Any())
            {
                list.AddRange(itemsToAdd);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds an item to <see cref="List{T}"/> <paramref name="numberOfTimesToAdd"/> times.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="itemToAdd"></param>
        /// <param name="numberOfTimesToAdd"></param>
        public static void Add<T>(this List<T> list, T itemToAdd, int numberOfTimesToAdd)
        {
            for (var i = 0; i < numberOfTimesToAdd; i++)
            {
                list.Add(itemToAdd);
            }
        }

        /// <summary>
        /// Shuffles a list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static List<T> Shuffle<T>(this List<T> list)
        {
            return list.OrderBy(a => Guid.NewGuid()).ToList();
        }
    }
}
