using System;
using System.Collections.Generic;
using System.Dynamic;
using NIST.CVP.Math;
using System.Numerics;

namespace NIST.CVP.Generation.Core.ExtensionMethods
{
    /// <summary>
    /// Extension methods for <see cref="ExpandoObject"/>
    /// </summary>
    public static class ExpandoObjectExtensions
    {
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
    }
}
