using System.ComponentModel;
using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.Symmetric.CTR.Enums
{
    public enum Cipher
    {
        [EnumMember(Value = "AES")]
        AES,

        [EnumMember(Value = "TDES")]
        TDES
    }
}
