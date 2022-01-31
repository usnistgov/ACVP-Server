using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums
{
    public enum HashFunctions
    {
        None,
        [EnumMember(Value = "SHA-1")]
        Sha1,
        [EnumMember(Value = "SHA2-224")]
        Sha2_d224,
        [EnumMember(Value = "SHA2-256")]
        Sha2_d256,
        [EnumMember(Value = "SHA2-384")]
        Sha2_d384,
        [EnumMember(Value = "SHA2-512")]
        Sha2_d512,
        [EnumMember(Value = "SHA2-512/224")]
        Sha2_d512t224,
        [EnumMember(Value = "SHA2-512/256")]
        Sha2_d512t256,
        [EnumMember(Value = "SHA3-224")]
        Sha3_d224,
        [EnumMember(Value = "SHA3-256")]
        Sha3_d256,
        [EnumMember(Value = "SHA3-384")]
        Sha3_d384,
        [EnumMember(Value = "SHA3-512")]
        Sha3_d512,
    }
}
