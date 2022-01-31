using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums
{
    public enum MacModes
    {
        None,
        [EnumMember(Value = "CMAC-AES128")]
        CMAC_AES128,
        [EnumMember(Value = "CMAC-AES192")]
        CMAC_AES192,
        [EnumMember(Value = "CMAC-AES256")]
        CMAC_AES256,
        [EnumMember(Value = "CMAC-TDES")]
        CMAC_TDES,
        [EnumMember(Value = "HMAC-SHA-1")]
        HMAC_SHA1,
        [EnumMember(Value = "HMAC-SHA2-224")]
        HMAC_SHA224,
        [EnumMember(Value = "HMAC-SHA2-256")]
        HMAC_SHA256,
        [EnumMember(Value = "HMAC-SHA2-384")]
        HMAC_SHA384,
        [EnumMember(Value = "HMAC-SHA2-512")]
        HMAC_SHA512,
        [EnumMember(Value = "HMAC-SHA2-512/224")]
        HMAC_SHA_d512t224,
        [EnumMember(Value = "HMAC-SHA2-512/256")]
        HMAC_SHA_d512t256,
        [EnumMember(Value = "HMAC-SHA3-224")]
        HMAC_SHA3_224,
        [EnumMember(Value = "HMAC-SHA3-256")]
        HMAC_SHA3_256,
        [EnumMember(Value = "HMAC-SHA3-384")]
        HMAC_SHA3_384,
        [EnumMember(Value = "HMAC-SHA3-512")]
        HMAC_SHA3_512,
    }
}
