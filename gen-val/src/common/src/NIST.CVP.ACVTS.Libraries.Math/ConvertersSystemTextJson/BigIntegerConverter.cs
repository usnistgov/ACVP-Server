using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Math.ConvertersSystemTextJson
{
    public class BigIntegerConverter : JsonConverter<BigInteger>
    {
        public override BigInteger Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return new BitString(reader.GetString()).ToPositiveBigInteger();
        }

        public override void Write(Utf8JsonWriter writer, BigInteger value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(new BitString(value).ToHex());
        }
    }
}
