using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.KAS.Enums
{
    public enum KasKdfOneStepAuxFunction
    {
        None,
        [EnumMember(Value = "SHA2-224")]
        SHA2_D224,
        [EnumMember(Value = "SHA2-256")]
        SHA2_D256,
        [EnumMember(Value = "SHA2-384")]
        SHA2_D384,
        [EnumMember(Value = "SHA2-512")]
        SHA2_D512,
        [EnumMember(Value = "SHA2-512/224")]
        SHA2_D512_T224,
        [EnumMember(Value = "SHA2-512/256")]
        SHA2_D512_T256,
        [EnumMember(Value = "SHA3-224")]
        SHA3_D224,
        [EnumMember(Value = "SHA3-256")]
        SHA3_D256,
        [EnumMember(Value = "SHA3-384")]
        SHA3_D384,
        [EnumMember(Value = "SHA3-512")]
        SHA3_D512,
        [EnumMember(Value = "HMAC-SHA2-224")]
        HMAC_SHA2_D224,
        [EnumMember(Value = "HMAC-SHA2-256")]
        HMAC_SHA2_D256,
        [EnumMember(Value = "HMAC-SHA2-384")]
        HMAC_SHA2_D384,
        [EnumMember(Value = "HMAC-SHA2-512")]
        HMAC_SHA2_D512,
        [EnumMember(Value = "HMAC-SHA2-512/224")]
        HMAC_SHA2_D512_T224,
        [EnumMember(Value = "HMAC-SHA2-512/256")]
        HMAC_SHA2_D512_T256,
        [EnumMember(Value = "HMAC-SHA3-224")]
        HMAC_SHA3_D224,
        [EnumMember(Value = "HMAC-SHA3-256")]
        HMAC_SHA3_D256,
        [EnumMember(Value = "HMAC-SHA3-384")]
        HMAC_SHA3_D384,
        [EnumMember(Value = "HMAC-SHA3-512")]        
        HMAC_SHA3_D512,
        [EnumMember(Value = "KMAC-128")]
        KMAC_128,
        [EnumMember(Value = "KMAC-256")]
        KMAC_256
    }
}