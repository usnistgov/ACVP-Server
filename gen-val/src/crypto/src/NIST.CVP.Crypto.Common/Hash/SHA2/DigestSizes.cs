using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.Hash.SHA2
{
    public enum DigestSizes
    {
        [EnumMember(Value = "160")]
        d160,
        [EnumMember(Value = "224")]
        d224,
        [EnumMember(Value = "256")]
        d256,
        [EnumMember(Value = "384")]
        d384,
        [EnumMember(Value = "512")]
        d512,
        [EnumMember(Value = "512/224")]
        d512t224,
        [EnumMember(Value = "512/256")]
        d512t256,
        [EnumMember(Value = "NONE")]
        NONE
    }
}