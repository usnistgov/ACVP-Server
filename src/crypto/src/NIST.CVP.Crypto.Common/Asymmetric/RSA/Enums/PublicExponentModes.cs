using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums
{
    public enum PublicExponentModes
    {
        Invalid,
        
        [EnumMember(Value = "fixed")]
        Fixed,

        [EnumMember(Value = "random")]
        Random
    }
}
