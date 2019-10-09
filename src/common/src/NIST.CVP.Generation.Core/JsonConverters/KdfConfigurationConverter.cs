using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfIkeV1;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfTwoStep;
using System;

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
            var typeString = (string)jo["kdfType"];
            var kdfType = EnumHelpers.GetEnumFromEnumDescription<KasKdf>(typeString, false);

            var obj = GetKdfConfiguration(kdfType, jo, serializer);

            return obj;
        }

        private object GetKdfConfiguration(KasKdf kdfType, JObject jo, JsonSerializer serializer)
        {
            switch (kdfType)
            {
                case KasKdf.None:
                    return null;
                case KasKdf.OneStep:
                    return jo.ToObject<OneStepConfiguration>(serializer);
                case KasKdf.TwoStep:
                    return jo.ToObject<TwoStepConfiguration>(serializer);
                case KasKdf.Ike_v1:
                    return jo.ToObject<IkeV1Configuration>(serializer);
                default:
                    throw new ArgumentException("No serializer exists for this kdf type");
            }
        }
    }
}