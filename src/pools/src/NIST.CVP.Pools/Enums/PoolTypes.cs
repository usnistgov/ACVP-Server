using System.Runtime.Serialization;

namespace NIST.CVP.Pools.Enums
{
    public enum PoolTypes
    {
        [EnumMember(Value = "sha")]
        SHA,

        [EnumMember(Value = "aes")]
        AES,

        [EnumMember(Value = "sha_mct")]
        SHA_MCT,

        [EnumMember(Value = "sha3_mct")]
        SHA3_MCT,

        [EnumMember(Value = "cshake_mct")]
        CSHAKE_MCT,

        [EnumMember(Value = "parallel_hash_mct")]
        PARALLEL_HASH_MCT,

        [EnumMember(Value = "tuple_hash_mct")]
        TUPLE_HASH_MCT,

        [EnumMember(Value = "aes_mct")]
        AES_MCT,

        [EnumMember(Value = "dsa_pqg")]
        DSA_PQG,

        [EnumMember(Value = "ecdsa_key")]
        ECDSA_KEY,

        [EnumMember(Value = "tdes_mct")]
        TDES_MCT,

        [EnumMember(Value = "rsa_key")]
        RSA_KEY
    }
}
