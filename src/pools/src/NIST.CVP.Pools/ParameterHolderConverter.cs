using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Enums;
using System;

namespace NIST.CVP.Pools
{
    public class ParameterHolderConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ParameterHolder);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);
            var type = (string) jo["type"];
            var holder = new ParameterHolder
            {
                Type = EnumHelpers.GetEnumFromEnumDescription<PoolTypes>(type)
            };

            holder.Parameters = GetParameters(holder.Type, jo["parameters"], serializer);
            holder.Result = GetResult(holder.Type, jo["result"], serializer);

            return holder;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var holder = (ParameterHolder)value;

            writer.WriteStartObject();
            writer.WritePropertyName("type");
            writer.WriteValue(EnumHelpers.GetEnumDescriptionFromEnum(holder.Type));
            writer.WritePropertyName("parameters");
            serializer.Serialize(writer, holder.Parameters);
            writer.WriteEndObject();
        }

        private IParameters GetParameters(PoolTypes typeId, JToken jo, JsonSerializer serializer)
        {
            switch (typeId)
            {
                case PoolTypes.SHA:
                case PoolTypes.SHA_MCT:
                    return jo.ToObject<ShaParameters>(serializer);
                case PoolTypes.AES:
                    return jo.ToObject<AesParameters>(serializer);

                default:
                    throw new Exception("Unable to parse parameters");
            }
        }

        private IResult GetResult(PoolTypes typeId, JToken jo, JsonSerializer serializer)
        {
            if (jo == null)
            {
                return null;
            }

            switch (typeId)
            {
                case PoolTypes.SHA:
                    return jo.ToObject<HashResult>(serializer);
                case PoolTypes.AES:
                    return jo.ToObject<AesResult>(serializer);
                case PoolTypes.SHA_MCT:
                    return jo.ToObject<MctResult<HashResult>>(serializer);

                default:
                    throw new Exception("Unable to parse result");
            }
        }
    }
}
