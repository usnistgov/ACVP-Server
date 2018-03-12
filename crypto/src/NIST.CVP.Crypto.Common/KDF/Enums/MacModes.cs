using System.ComponentModel;
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
        [EnumMember(Value = "HMAC-SHA1")]
        HMAC_SHA1,
        [EnumMember(Value = "HMAC-SHA224")]
        HMAC_SHA224,
        [EnumMember(Value = "HMAC-SHA256")]
        HMAC_SHA256,
        [EnumMember(Value = "HMAC-SHA384")]
        HMAC_SHA384,
        [EnumMember(Value = "HMAC-SHA512")]
        HMAC_SHA512,
    }
}
