using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfIkeV1;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfIkeV2;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfTls10_11;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfTls12;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfTwoStep;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters
{
    public class KdfParameterConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IKdfParameter);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);
            var typeString = (string)jo["kdfType"];
            var kdfType = EnumHelpers.GetEnumFromEnumDescription<Kda>(typeString, false);

            var obj = GetKdfParameter(kdfType, jo, serializer);

            return obj;
        }

        private object GetKdfParameter(Kda kdfType, JObject jo, JsonSerializer serializer)
        {
            switch (kdfType)
            {
                case Kda.None:
                    return null;
                case Kda.OneStep:
                    return jo.ToObject<KdfParameterOneStep>(serializer);
                case Kda.OneStepNoCounter:
                    return jo.ToObject<KdfParameterOneStepNoCounter>(serializer);
                case Kda.TwoStep:
                    return jo.ToObject<KdfParameterTwoStep>(serializer);
                case Kda.Ike_v1:
                    return jo.ToObject<KdfParameterIkeV1>(serializer);
                case Kda.Ike_v2:
                    return jo.ToObject<KdfParameterIkeV2>(serializer);
                case Kda.Tls_v10_v11:
                    return jo.ToObject<KdfParameterTls10_11>(serializer);
                case Kda.Tls_v12:
                    return jo.ToObject<KdfParameterTls12>(serializer);
                default:
                    throw new ArgumentException("No serializer exists for this kdf type");
            }
        }
    }
}
