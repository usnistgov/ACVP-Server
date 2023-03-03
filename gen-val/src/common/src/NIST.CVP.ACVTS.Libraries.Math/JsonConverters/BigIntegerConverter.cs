using System;
using System.Numerics;
using Newtonsoft.Json;

namespace NIST.CVP.ACVTS.Libraries.Math.JsonConverters
{
    public class BigIntegerConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var bigint = (BigInteger)value;

            writer.WriteValue(new BitString(bigint).ToHex());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Use the hex representation constructor
            return new BitString((string)reader.Value).ToPositiveBigInteger();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(BigInteger);
        }
    }
}
