using System.ComponentModel;
using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums
{
    public enum PrivateKeyModes
    {
        [EnumMember(Value = "standard")]
        Standard,

        [EnumMember(Value = "crt")]
        Crt
    }
}
