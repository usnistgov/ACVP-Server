using System;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Symmetric.AES;

namespace NIST.CVP.Generation.AES_CFB1
{
    public class BitOrientedBitStringConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            BitOrientedBitString bs = (BitOrientedBitString)value;
            var printedValue = new string(bs.ToString().Replace(" ", string.Empty).ToArray());

            writer.WriteValue(printedValue);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return BitOrientedBitString.GetBitStringEachCharacterOfInputIsBit((string)reader.Value);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(BitOrientedBitString);
        }
    }
}
