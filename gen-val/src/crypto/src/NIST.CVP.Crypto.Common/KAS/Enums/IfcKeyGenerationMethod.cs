using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.KAS.Enums
{
    public enum IfcKeyGenerationMethod
    {
        None,
        [EnumMember(Value = "rsakpg1-basic")]
        RsaKpg1_basic,
        [EnumMember(Value = "rsakpg1-prime-factor")]
        RsaKpg1_primeFactor,
        [EnumMember(Value = "rsakpg1-crt")]
        RsaKpg1_crt,
        [EnumMember(Value = "rsakpg2-basic")]
        RsaKpg2_basic,
        [EnumMember(Value = "rsakpg2-prime-factor")]
        RsaKpg2_primeFactor,
        [EnumMember(Value = "rsakpg2-crt")]
        RsaKpg2_crt
    }
}