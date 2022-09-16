using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums
{
    public enum PrimeTestModes
    {
        [EnumMember(Value = "invalid")]
        Invalid,

        [EnumMember(Value = "2pow100")]
        TwoPow100ErrorBound,

        [EnumMember(Value = "2powSecStr")]
        TwoPowSecurityStrengthErrorBound
    }
}
