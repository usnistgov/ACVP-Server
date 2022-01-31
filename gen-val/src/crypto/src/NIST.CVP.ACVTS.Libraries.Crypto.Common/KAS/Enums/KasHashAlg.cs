using System;
using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums
{
    public enum KasHashAlg
    {
        [EnumMember(Value = "SHA-1")]
        SHA1,
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
    }
}
