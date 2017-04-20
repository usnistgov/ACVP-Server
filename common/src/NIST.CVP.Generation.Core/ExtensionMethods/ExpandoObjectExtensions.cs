using System.Collections.Generic;
using System.Dynamic;
using NIST.CVP.Math;


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
            var valueAsBitString = sourcePropertyValue as BitString;
            if (valueAsBitString != null)
            {
                return valueAsBitString;
            }
            return new BitString(sourcePropertyValue.ToString());
        }
    }
}
