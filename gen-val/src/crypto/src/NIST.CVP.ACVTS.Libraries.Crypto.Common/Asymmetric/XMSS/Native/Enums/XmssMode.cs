using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Enums
{
    /// <summary>
    /// XMSS parameter set labels from https://nvlpubs.nist.gov/nistpubs/SpecialPublications/NIST.SP.800-208.pdf
    /// </summary>
    public enum XmssMode
    {
        Invalid,

        [EnumMember(Value = "XMSS-SHA2_10_256")]
        XMSS_SHA2_10_256,
        [EnumMember(Value = "XMSS-SHA2_16_256")]
        XMSS_SHA2_16_256,
        [EnumMember(Value = "XMSS-SHA2_20_256")]
        XMSS_SHA2_20_256,

        [EnumMember(Value = "XMSS-SHA2_10_192")]
        XMSS_SHA2_10_192,
        [EnumMember(Value = "XMSS-SHA2_16_192")]
        XMSS_SHA2_16_192,
        [EnumMember(Value = "XMSS-SHA2_20_192")]
        XMSS_SHA2_20_192,

        [EnumMember(Value = "XMSS-SHAKE256_10_256")]
        XMSS_SHAKE256_10_256,
        [EnumMember(Value = "XMSS-SHAKE256_16_256")]
        XMSS_SHAKE256_16_256,
        [EnumMember(Value = "XMSS-SHAKE256_20_256")]
        XMSS_SHAKE256_20_256,

        [EnumMember(Value = "XMSS-SHAKE256_10_192")]
        XMSS_SHAKE256_10_192,
        [EnumMember(Value = "XMSS-SHAKE256_16_192")]
        XMSS_SHAKE256_16_192,
        [EnumMember(Value = "XMSS-SHAKE256_20_192")]
        XMSS_SHAKE256_20_192
    }
}
