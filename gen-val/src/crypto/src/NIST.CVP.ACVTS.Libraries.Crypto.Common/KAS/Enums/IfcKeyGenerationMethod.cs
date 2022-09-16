using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums
{
    public enum IfcKeyGenerationMethod
    {
        [EnumMember(Value = "none")]
        None,
        /// <summary>
        /// Fixed public exponent private key in basic format
        /// </summary>
        [EnumMember(Value = "rsakpg1-basic")]
        RsaKpg1_basic,
        /// <summary>
        /// Fixed public exponent private key in prime factor format
        /// </summary>
        [EnumMember(Value = "rsakpg1-prime-factor")]
        RsaKpg1_primeFactor,
        /// <summary>
        /// Fixed public exponent private key in CRT format
        /// </summary>
        [EnumMember(Value = "rsakpg1-crt")]
        RsaKpg1_crt,
        /// <summary>
        /// Random public exponent private key in basic format
        /// </summary>
        [EnumMember(Value = "rsakpg2-basic")]
        RsaKpg2_basic,
        /// <summary>
        /// Random public exponent private key in prime factor format
        /// </summary>
        [EnumMember(Value = "rsakpg2-prime-factor")]
        RsaKpg2_primeFactor,
        /// <summary>
        /// Random public exponent private key in CRT format
        /// </summary>
        [EnumMember(Value = "rsakpg2-crt")]
        RsaKpg2_crt
    }
}
