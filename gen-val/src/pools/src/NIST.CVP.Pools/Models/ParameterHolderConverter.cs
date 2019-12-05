using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using System;
using PoolTypes = NIST.CVP.Pools.Enums.PoolTypes;

namespace NIST.CVP.Pools.Models
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
                case PoolTypes.AES_MCT:
                    return jo.ToObject<AesParameters>(serializer);

                case PoolTypes.TDES_MCT:
                    return jo.ToObject<TdesParameters>(serializer);

                case PoolTypes.CSHAKE_MCT:
                    return jo.ToObject<CShakeParameters>(serializer);

                case PoolTypes.DSA_PQG:
                    return jo.ToObject<DsaDomainParametersParameters>(serializer);

                case PoolTypes.PARALLEL_HASH_MCT:
                    return jo.ToObject<ParallelHashParameters>(serializer);

                case PoolTypes.RSA_KEY:
                    return jo.ToObject<RsaKeyParameters>(serializer);

                case PoolTypes.ECDSA_KEY:
                    return jo.ToObject<EcdsaKeyParameters>(serializer);

                case PoolTypes.TUPLE_HASH_MCT:
                    return jo.ToObject<TupleHashParameters>(serializer);

                case PoolTypes.SHA3_MCT:
                    return jo.ToObject<Sha3Parameters>(serializer);

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
                case PoolTypes.SHA3_MCT:
                    return jo.ToObject<MctResult<HashResult>>(serializer);

                case PoolTypes.AES_MCT:
                    return jo.ToObject<MctResult<AesResult>>(serializer);

                case PoolTypes.TDES_MCT:
                    return jo.ToObject<MctResult<TdesResult>>(serializer);

                case PoolTypes.CSHAKE_MCT:
                    return jo.ToObject<MctResult<CShakeResult>>(serializer);

                case PoolTypes.DSA_PQG:
                    return jo.ToObject<DsaDomainParametersResult>(serializer);

                case PoolTypes.PARALLEL_HASH_MCT:
                    return jo.ToObject<MctResult<ParallelHashResult>>(serializer);

                case PoolTypes.RSA_KEY:
                    return jo.ToObject<RsaPrimeResult>(serializer);

                case PoolTypes.ECDSA_KEY:
                    return jo.ToObject<EcdsaKeyResult>(serializer);

                case PoolTypes.TUPLE_HASH_MCT:
                    return jo.ToObject<MctResult<TupleHashResult>>(serializer);

                default:
                    throw new Exception("Unable to parse result");
            }
        }
    }
}
