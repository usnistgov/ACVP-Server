using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums
{
    public enum PublicExponentModes
    {
        [EnumMember(Value = "invalid")]
        Invalid,

        [EnumMember(Value = "fixed")]
        Fixed,

        [EnumMember(Value = "random")]
        Random
    }
}
