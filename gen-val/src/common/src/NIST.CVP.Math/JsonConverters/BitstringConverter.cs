using Newtonsoft.Json;
using NIST.CVP.Math;
using System;

namespace NIST.CVP.Generation.Core.JsonConverters
{
    public class BitstringConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var bs = (BitString)value;

            writer.WriteValue(bs.ToHex());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            //use the hex representation constructor
            return new BitString((string)reader.Value);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(BitString);
        }
    }
}
