using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums
{
    public enum BlockCipherDirections
    {
        [EnumMember(Value = "encrypt")]
        Encrypt,

        [EnumMember(Value = "decrypt")]
        Decrypt
    }
}
