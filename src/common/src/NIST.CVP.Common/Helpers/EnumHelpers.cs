using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace NIST.CVP.Common.Helpers
{
    public static class EnumHelpers
    {
        /// <summary>
        /// Gets the description attribute from an enum.  
        /// Returns the enum.ToString() when no description found.
        /// </summary>
        /// <param name="enumToGetDescriptionFrom">The enum to retrieve the description from.</param>
        /// <returns></returns>
        public static string GetEnumDescriptionFromEnum(Enum enumToGetDescriptionFrom)
        {
            FieldInfo fi = enumToGetDescriptionFrom.GetType().GetField(enumToGetDescriptionFrom.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                    typeof(DescriptionAttribute),
                    false);

            if (attributes != null &&
                attributes.Length > 0)
            {
                return attributes[0].Description;
            }

            return enumToGetDescriptionFrom.ToString();
        }

        /// <summary>
        /// Gets the enum of type <see cref="T"/> matching the description.
        /// </summary>
        /// <typeparam name="T">The enum type to return/parse descriptions of.</typeparam>
        /// <param name="enumDescription">The description to search the enum for.</param>
        /// <param name="shouldThrow">Should the method throw if the enum is not found?</param>
        /// <returns></returns>
        public static T GetEnumFromEnumDescription<T>(string enumDescription, bool shouldThrow = true)
            where T : struct
        {
            if (!typeof(T).IsEnum)
            {
                throw new InvalidOperationException("Type is not an enum");
            }

            try
            {
                return (T) typeof(T)
                    .GetFields()
                    .First(
                        f => f.GetCustomAttributes<DescriptionAttribute>()
                            .Any(a => a.Description.Equals(enumDescription, StringComparison.OrdinalIgnoreCase))
                    )
                    .GetValue(null);
            }
            catch (Exception)
            {
                if (shouldThrow)
                {
                    throw;
                }

                return default(T);
            }
        }

        /// <summary>
        /// Gets the description attributes from an enum type.
        /// If a description is not found for any items in the enum, 
        /// the ToString representation of that item is returned.
        /// </summary>
        /// <typeparam name="T">The enum type to get descriptions from</typeparam>
        /// <returns></returns>
        public static List<string> GetEnumDescriptions<T>()
            where T : struct, IConvertible
        {
            var type = typeof(T);

            if (!type.IsEnum)
            {
                throw new ArgumentException("Only Enum types allowed");
            }

            List<string> descriptions = new List<string>();
            foreach (var value in Enum.GetValues(type).Cast<Enum>())
            {
                descriptions.Add(GetEnumDescriptionFromEnum(value));
            }

            return descriptions;
        }
    }
}
