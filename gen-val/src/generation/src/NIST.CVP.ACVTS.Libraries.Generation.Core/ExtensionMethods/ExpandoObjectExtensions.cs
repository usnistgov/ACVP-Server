using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Helpers;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core.ExtensionMethods
{
    /// <summary>
    /// Extension methods for <see cref="ExpandoObject"/>
    /// </summary>
    public static class ExpandoObjectExtensions
    {
        /// <summary>
        /// Determines if the expando object contains a property
        /// </summary>
        /// <param name="source">The expando object to check against</param>
        /// <param name="propertyName">The property to check for</param>
        /// <returns></returns>
        public static bool ContainsProperty(this ExpandoObject source, string propertyName)
        {
            var dict = (IDictionary<string, object>)source;
            return dict.ContainsKey(propertyName);
        }

        /// <summary>
        /// Gets a <see cref="BitString"/> from <see cref="source"/>, if not successfully parsed, returns null.
        /// </summary>
        /// <param name="source">The <see cref="ExpandoObject"/> to parse for <see cref="sourcePropertyName"/></param>
        /// <param name="sourcePropertyName">The name of the property that should be parsed as a <see cref="BitString"/></param>
        /// <returns></returns>
        public static BitString GetBitStringFromProperty(this ExpandoObject source, string sourcePropertyName)
        {
            if (!source.ContainsProperty(sourcePropertyName))
            {
                return null;
            }

            var sourcePropertyValue = ((IDictionary<string, object>)source)[sourcePropertyName];
            if (sourcePropertyValue == null)
            {
                return null;
            }

            if (sourcePropertyValue is BitString valueAsBitString)
            {
                return valueAsBitString;
            }

            return new BitString(sourcePropertyValue.ToString());
        }

        /// <summary>
        /// Gets a <see cref="BigInteger"/> from <see cref="source"/>, if not successfully parsed, returns 0.
        /// </summary>
        /// <param name="source">The <see cref="ExpandoObject"/> to parse for <see cref="sourcePropertyName"/></param>
        /// <param name="sourcePropertyName">The name of the property that should be parsed as a <see cref="BigInteger"/></param>
        /// <returns></returns>
        public static BigInteger GetBigIntegerFromProperty(this ExpandoObject source, string sourcePropertyName)
        {
            if (!source.ContainsProperty(sourcePropertyName))
            {
                return 0;
            }

            var sourcePropertyValue = ((IDictionary<string, object>)source)[sourcePropertyName];
            if (sourcePropertyValue == null)
            {
                return 0;
            }

            if (sourcePropertyValue is string)
            {
                return new BitString(sourcePropertyValue.ToString()).ToPositiveBigInteger();
            }

            var valueAsBigInteger = (BigInteger)sourcePropertyValue;
            if (valueAsBigInteger != 0)
            {
                return valueAsBigInteger;
            }

            return 0;
        }

        /// <summary>
        /// Gets the property from the specified type (or default value)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="sourcePropertyName"></param>
        /// <returns></returns>
        public static T GetTypeFromProperty<T>(this ExpandoObject source, string sourcePropertyName)

        {
            if (!source.ContainsProperty(sourcePropertyName))
            {
                return default(T);
            }

            var sourcePropertyValue = ((IDictionary<string, object>)source)[sourcePropertyName];
            if (sourcePropertyValue == null)
            {
                return default(T);
            }

            try
            {
                if (typeof(T).IsEnum)
                {
                    T result = (T)Enum.Parse(typeof(T), sourcePropertyValue.ToString());
                    if (!Enum.IsDefined(typeof(T), result)) return default(T);
                    return result;
                }
                else
                {
                    return (T)Convert.ChangeType(sourcePropertyValue, typeof(T));
                }
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        /// <summary>
        /// Adds a <see cref="BigInteger"/> to the <see cref="source"/> when it is not zero.
        /// </summary>
        /// <param name="source">The expando object to add the BigInteger to</param>
        /// <param name="label">The key to add to the expando object</param>
        /// <param name="value">The value to add</param>
        public static void AddBigIntegerWhenNotZero(this ExpandoObject source, string label, BigInteger value)
        {
            DynamicHelpers.AddBigIntegerToDynamicWhenNotZero(source, label, value);
        }

        /// <summary>
        /// Adds a <see cref="int"/> to the <see cref="source"/> when it is not zero.
        /// </summary>
        /// <param name="source">The expando object to add the int to</param>
        /// <param name="label">The key to add to the expando object</param>
        /// <param name="value">The value to add</param>
        public static void AddIntegerWhenNotZero(this ExpandoObject source, string label, int value)
        {
            DynamicHelpers.AddIntegerToDynamicWhenNotZero(source, label, value);
        }
    }
}
