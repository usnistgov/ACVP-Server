using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums
{
    public enum BlockCipherDirections
    {    
        None,
        
        [EnumMember(Value = "encrypt")]
        Encrypt,

        [EnumMember(Value = "decrypt")]
        Decrypt
    }
}
