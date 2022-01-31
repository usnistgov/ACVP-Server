using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums
{
    public enum Fips186Standard
    {
        None,

        [EnumMember(Value = "FIPS186-2")]
        Fips186_2,

        [EnumMember(Value = "FIPS186-4")]
        Fips186_4,

        [EnumMember(Value = "FIPS186-5")]
        Fips186_5
    }
}
