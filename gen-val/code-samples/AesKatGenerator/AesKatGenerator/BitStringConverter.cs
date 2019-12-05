using System;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using NIST.CVP.Math;

namespace AesKatGenerator
{
    public class BitStringConverter : ITypeConverter
    {
        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            return ((BitString)value).ToHex();
        }

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            throw new NotImplementedException();
        }
    }
}
