using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums
{
    public enum PrimeGenModes
    {
        [EnumMember(Value = "B.3.2")]
        B32,

        [EnumMember(Value = "B.3.3")]
        B33,

        [EnumMember(Value = "B.3.4")]
        B34,

        [EnumMember(Value = "B.3.5")]
        B35,

        [EnumMember(Value = "B.3.6")]
        B36
    }
}
