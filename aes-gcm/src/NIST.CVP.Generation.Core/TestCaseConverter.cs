using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.Core
{
    public class TestCaseConverter:JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
           BitString bs = (BitString)value;

            writer.WriteValue(bs.ToHex());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            //use the hex representation constructor
            return new BitString((string)reader.Value);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ITestCase);
        }
    }
}
