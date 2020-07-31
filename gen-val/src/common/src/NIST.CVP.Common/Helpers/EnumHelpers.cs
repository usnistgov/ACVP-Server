using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using NLog;

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

            EnumMemberAttribute[] attributes =
                (EnumMemberAttribute[])fi.GetCustomAttributes(
                    typeof(EnumMemberAttribute),
                    false);

            if (attributes != null &&
                attributes.Length > 0)
            {
                return attributes[0].Value;
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
                        f => f.GetCustomAttributes<EnumMemberAttribute>()
                            .Any(a => a.Value.Equals(enumDescription, StringComparison.OrdinalIgnoreCase))
                    )
                    .GetValue(null);
            }
            catch (Exception)
            {
                if (shouldThrow)
                {
                    ThisLogger.Error($"Couldn't find an {typeof(T)} matching {nameof(enumDescription)} of \"{enumDescription}\"");
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
        
        public static List<T> GetEnums<T>()
            where T : struct, IConvertible
        {
            var type = typeof(T);

            if (!type.IsEnum)
            {
                throw new ArgumentException("Only Enum types allowed");
            }

            var enums = new List<T>();
            foreach (var value in Enum.GetValues(type).Cast<T>())
            {
                enums.Add(value);
            }

            return enums;
        }
        
        public static List<T> GetEnumsWithoutDefault<T>()
            where T : struct, IConvertible
        {
            var type = typeof(T);

            if (!type.IsEnum)
            {
                throw new ArgumentException("Only Enum types allowed");
            }

            var enums = new List<T>();
            foreach (var value in Enum.GetValues(type).Cast<T>())
            {
                if (value.Equals(default(T))) continue;
                
                enums.Add(value);
            }

            return enums;
        }

        private static Logger ThisLogger = LogManager.GetCurrentClassLogger();
    }
}
