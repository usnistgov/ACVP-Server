using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;

namespace NIST.CVP.Generation.Core.JsonConverters
{
    public class KdfConfigurationConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IKdfConfiguration);
        }
        
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);
            var typeString = (string) jo[nameof(IKdfConfiguration.KdfType)];
            var kdfType = EnumHelpers.GetEnumFromEnumDescription<KasKdf>(typeString);
            
            return GetKdfConfiguration(kdfType, jo, serializer);
        }

        private object GetKdfConfiguration(KasKdf kdfType, JObject jo, JsonSerializer serializer)
        {
            switch (kdfType)
            {
                case KasKdf.None:
                    return null;
                case KasKdf.OneStep:
                    return jo.ToObject<OneStepConfiguration>(serializer);
                default:
                    throw new ArgumentException("No serializer exists for this kdf type");
            }
        }
    }
}