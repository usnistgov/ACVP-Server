using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums
{
    public enum PrivateKeyModes
    {
        [EnumMember(Value = "invalid")]
        Invalid,

        [EnumMember(Value = "standard")]
        Standard,

        [EnumMember(Value = "crt")]
        Crt
    }
}
