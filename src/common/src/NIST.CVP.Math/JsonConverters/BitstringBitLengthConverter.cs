using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace NIST.CVP.Math.JsonConverters
{
    public class BitstringBitLengthConverter : JsonConverter
    {

        private class BitStringModel
        {
            public string Value { get; set; }
            public int BitLength { get; set; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var bs = (BitString)value;

            writer.WriteStartObject();
            writer.WritePropertyName(nameof(BitStringModel.Value));
            writer.WriteValue(bs.ToHex());
            writer.WritePropertyName(nameof(BitStringModel.BitLength));
            writer.WriteValue(bs.BitLength);
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);

            var model = JsonConvert.DeserializeObject<BitStringModel>(
                jObject.ToString(),
                new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }
            );

            return new BitString(model.Value, model.BitLength);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(BitString);
        }
    }
}
