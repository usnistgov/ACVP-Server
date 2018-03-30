using System.ComponentModel;

namespace NIST.CVP.Crypto.Common.KDF.Enums
{
    public enum MacModes
    {
        [Description("CMAC-AES128")]
        CMAC_AES128,
        [Description("CMAC-AES192")]
        CMAC_AES192,
        [Description("CMAC-AES256")]
        CMAC_AES256,
        [Description("CMAC-TDES")]
        CMAC_TDES,
        [Description("HMAC-SHA1")]
        HMAC_SHA1,
        [Description("HMAC-SHA224")]
        HMAC_SHA224,
        [Description("HMAC-SHA256")]
        HMAC_SHA256,
        [Description("HMAC-SHA384")]
        HMAC_SHA384,
        [Description("HMAC-SHA512")]
        HMAC_SHA512,
    }
}
