using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Math.ConvertersSystemTextJson
{
    public class BitStringConverter : JsonConverter<BitString>
    {
        public override BitString Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return new(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, BitString value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToHex());
        }
    }
}
