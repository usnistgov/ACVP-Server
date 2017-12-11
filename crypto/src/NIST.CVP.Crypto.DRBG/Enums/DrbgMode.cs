using System.ComponentModel;

namespace NIST.CVP.Crypto.DRBG.Enums
{
    public enum DrbgMode
    {
        [Description("AES-128")]
        AES128,

        [Description("AES-192")]
        AES192,

        [Description("AES-256")]
        AES256,

        [Description("TDES")]
        TDES,

        [Description("SHA-1")]
        SHA1,

        [Description("SHA2-224")]
        SHA224,

        [Description("SHA2-256")]
        SHA256,

        [Description("SHA2-384")]
        SHA384,

        [Description("SHA2-512")]
        SHA512,

        [Description("SHA2-512/224")]
        SHA512t224,

        [Description("SHA2-512/256")]
        SHA512t256,
    }
}