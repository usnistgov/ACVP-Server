using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Enums
{
    /// <summary>
    /// LM-OTS labels from https://nvlpubs.nist.gov/nistpubs/SpecialPublications/NIST.SP.800-208.pdf 
    /// </summary>
    public enum LmOtsMode
    {
        Invalid,

        [EnumMember(Value = "LMOTS_SHA256_N24_W1")]
        LMOTS_SHA256_N24_W1,
        [EnumMember(Value = "LMOTS_SHA256_N24_W2")]
        LMOTS_SHA256_N24_W2,
        [EnumMember(Value = "LMOTS_SHA256_N24_W4")]
        LMOTS_SHA256_N24_W4,
        [EnumMember(Value = "LMOTS_SHA256_N24_W8")]
        LMOTS_SHA256_N24_W8,

        [EnumMember(Value = "LMOTS_SHA256_N32_W1")]
        LMOTS_SHA256_N32_W1,
        [EnumMember(Value = "LMOTS_SHA256_N32_W2")]
        LMOTS_SHA256_N32_W2,
        [EnumMember(Value = "LMOTS_SHA256_N32_W4")]
        LMOTS_SHA256_N32_W4,
        [EnumMember(Value = "LMOTS_SHA256_N32_W8")]
        LMOTS_SHA256_N32_W8,

        [EnumMember(Value = "LMOTS_SHAKE_N24_W1")]
        LMOTS_SHAKE_N24_W1,
        [EnumMember(Value = "LMOTS_SHAKE_N24_W2")]
        LMOTS_SHAKE_N24_W2,
        [EnumMember(Value = "LMOTS_SHAKE_N24_W4")]
        LMOTS_SHAKE_N24_W4,
        [EnumMember(Value = "LMOTS_SHAKE_N24_W8")]
        LMOTS_SHAKE_N24_W8,

        [EnumMember(Value = "LMOTS_SHAKE_N32_W1")]
        LMOTS_SHAKE_N32_W1,
        [EnumMember(Value = "LMOTS_SHAKE_N32_W2")]
        LMOTS_SHAKE_N32_W2,
        [EnumMember(Value = "LMOTS_SHAKE_N32_W4")]
        LMOTS_SHAKE_N32_W4,
        [EnumMember(Value = "LMOTS_SHAKE_N32_W8")]
        LMOTS_SHAKE_N32_W8
    }
}
