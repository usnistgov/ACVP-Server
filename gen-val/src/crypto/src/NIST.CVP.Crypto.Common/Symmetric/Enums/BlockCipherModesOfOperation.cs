using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.Symmetric.Enums
{
    public enum BlockCipherModesOfOperation
    {
        [EnumMember(Value = "ecb")]
        Ecb,

        [EnumMember(Value = "cbc")]
        Cbc,

        [EnumMember(Value = "cbci")]
        Cbci,

        [EnumMember(Value = "cbcCs1")]
        CbcCs1,

        [EnumMember(Value = "cbcCs2")]
        CbcCs2,

        [EnumMember(Value = "cbcCs3")]
        CbcCs3,

        [EnumMember(Value = "cbcmac")]
        CbcMac,

        [EnumMember(Value = "ccm")]
        Ccm,

        [EnumMember(Value = "cfbbit")]
        CfbBit,

        [EnumMember(Value = "cfbbyte")]
        CfbByte,

        [EnumMember(Value = "cfbblock")]
        CfbBlock,

        [EnumMember(Value = "cfbpbit")]
        CfbpBit,

        [EnumMember(Value = "cfbpbyte")]
        CfbpByte,

        [EnumMember(Value = "cfbpblock")]
        CfbpBlock,

        [EnumMember(Value = "ctr")]
        Ctr,

        [EnumMember(Value = "gcm")]
        Gcm,

        [EnumMember(Value = "ofb")]
        Ofb,

        [EnumMember(Value = "ofbi")]
        Ofbi,

        [EnumMember(Value = "xts")]
        Xts,

        [EnumMember(Value = "gcmsiv")]
        GcmSiv
    }
}