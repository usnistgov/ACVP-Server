using System.ComponentModel;
using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums
{
    public enum PublicExponentModes
    {
        [EnumMember(Value = "fixed")]
        Fixed,

        [EnumMember(Value = "random")]
        Random
    }
}
