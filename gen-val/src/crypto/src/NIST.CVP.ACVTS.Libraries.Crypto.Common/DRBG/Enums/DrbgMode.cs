using System.ComponentModel;
using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG.Enums
{
    public enum DrbgMode
    {
        [EnumMember(Value = "AES-128")]
        AES128,

        [EnumMember(Value = "AES-192")]
        AES192,

        [EnumMember(Value = "AES-256")]
        AES256,

        [EnumMember(Value = "TDES")]
        TDES,

        [EnumMember(Value = "SHA-1")]
        SHA1,

        [EnumMember(Value = "SHA2-224")]
        SHA224,

        [EnumMember(Value = "SHA2-256")]
        SHA256,

        [EnumMember(Value = "SHA2-384")]
        SHA384,

        [EnumMember(Value = "SHA2-512")]
        SHA512,

        [EnumMember(Value = "SHA2-512/224")]
        SHA512t224,

        [EnumMember(Value = "SHA2-512/256")]
        SHA512t256,
    }
}
