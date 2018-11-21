using System.ComponentModel;
using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.KDF.Components.SSH.Enums
{
    public enum Cipher
    {
        [EnumMember(Value = "TDES")]
        TDES,

        [EnumMember(Value = "AES-128")]
        AES128,

        [EnumMember(Value = "AES-192")]
        AES192,

        [EnumMember(Value = "AES-256")]
        AES256
    }
}
