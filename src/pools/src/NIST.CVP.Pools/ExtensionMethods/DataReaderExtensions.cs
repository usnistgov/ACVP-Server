using System;
using System.Data;

namespace NIST.CVP.Pools.ExtensionMethods
{
    public static class DataReaderExtensions
    {
        public static DateTime? GetNullableDateTime(this IDataReader reader, string fieldName)
        {
            var x = reader.GetOrdinal(fieldName);
            return reader.IsDBNull(x) ? (DateTime?) null : reader.GetDateTime(x);
        }
    }
}
