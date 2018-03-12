using System.ComponentModel;
using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.KDF.Components.SSH.Enums
{
    public enum Cipher
    {
        [EnumMember(Value = "tdes")]
        TDES,

        [EnumMember(Value = "aes-128")]
        AES128,

        [EnumMember(Value = "aes-192")]
        AES192,

        [EnumMember(Value = "aes-256")]
        AES256
    }
}
