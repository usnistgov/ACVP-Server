using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Common.Oracle.ParameterTypes;
using System;

namespace NIST.CVP.Common.Oracle
{
    public class ParametersConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PoolProperties);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);
            var holder = new ParameterHolder
            {
                TypeId = (int)jo["typeId"]
            };

            holder.Parameters = GetParameters(holder.TypeId, jo["parameters"], serializer);

            return new PoolProperties
            {
                FilePath = (string) jo["filePath"],
                MaxCapacity = (int) jo["maxCapacity"],
                MonitorFrequency = (int) jo["monitorFrequency"],
                Parameters = holder
            };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        private Parameters GetParameters(int typeId, JToken jo, JsonSerializer serializer)
        {
            switch (typeId)
            {
                case 1:
                    return jo.ToObject<ShaParameters>(serializer);
                case 2:
                    return jo.ToObject<AesParameters>(serializer);

                default:
                    throw new Exception("Unable to parse parameters");
            }
        }
    }
}
