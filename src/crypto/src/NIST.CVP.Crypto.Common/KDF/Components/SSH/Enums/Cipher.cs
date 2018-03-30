using System.ComponentModel;

namespace NIST.CVP.Crypto.Common.KDF.Components.SSH.Enums
{
    public enum Cipher
    {
        [Description("tdes")]
        TDES,

        [Description("aes-128")]
        AES128,

        [Description("aes-192")]
        AES192,

        [Description("aes-256")]
        AES256
    }
}
