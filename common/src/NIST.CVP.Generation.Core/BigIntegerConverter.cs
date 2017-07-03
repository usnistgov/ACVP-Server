using System;
using System.Numerics;
using Newtonsoft.Json;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.Core
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
            //use the hex representation constructor
            return new BitString((string)reader.Value).ToPositiveBigInteger();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(BigInteger);
        }
    }
}