using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums
{
    public enum PrivateKeyModes
    {
        Invalid,

        [EnumMember(Value = "standard")]
        Standard,

        [EnumMember(Value = "crt")]
        Crt
    }
}
