using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
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

            return holder;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        private IParameters GetParameters(PoolTypes typeId, JToken jo, JsonSerializer serializer)
        {
            // TODO need one for each parameter type
            switch (typeId)
            {
                case PoolTypes.SHA:
                    return jo.ToObject<ShaParameters>(serializer);
                case PoolTypes.AES:
                    return jo.ToObject<AesParameters>(serializer);

                default:
                    throw new Exception("Unable to parse parameters");
            }
        }
    }
}
