using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.Core
{
    public static class DynamicPropertyChecker
    {
        public static bool ContainsProperty(this ExpandoObject source, string propertyName)
        {
            var dict = (IDictionary<string, object>)source;
            return dict.ContainsKey(propertyName);
        }
    }
}
