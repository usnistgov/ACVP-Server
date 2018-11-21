using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums
{
    public enum PrimeTestModes
    {
        [EnumMember(Value = "none")]
        None,

        [EnumMember(Value = "tblC2")]
        C2,

        [EnumMember(Value = "tblC3")]
        C3
    }
}
