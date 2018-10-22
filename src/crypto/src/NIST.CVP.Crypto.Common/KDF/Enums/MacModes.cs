using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.KDF.Enums
{
    public enum MacModes
    {
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
    }
}
