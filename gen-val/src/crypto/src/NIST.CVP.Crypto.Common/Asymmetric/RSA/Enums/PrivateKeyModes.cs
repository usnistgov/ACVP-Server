using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums
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
