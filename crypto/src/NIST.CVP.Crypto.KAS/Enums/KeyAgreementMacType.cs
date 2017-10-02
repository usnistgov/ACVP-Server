using System.ComponentModel;

namespace NIST.CVP.Crypto.KAS.Enums
{
    public enum KeyAgreementMacType
    {
        /// <summary>
        /// MAC not used
        /// </summary>
        None,
        /// <summary>
        /// HMAC - SHA2-224
        /// </summary>
        [Description("hmac-sha2-224")]
        HmacSha2D224,
        /// <summary>
        /// HMAC - SHA2-256
        /// </summary>
        [Description("hmac-sha2-256")]
        HmacSha2D256,
        /// <summary>
        /// HMAC - SHA2-384
        /// </summary>
        [Description("hmac-sha2-384")]
        HmacSha2D384,
        /// <summary>
        /// HMAC - SHA2-512
        /// </summary>
        [Description("hmac-sha2-512")]
        HmacSha2D512,
        /// <summary>
        /// CMAC-AES
        /// </summary>
        [Description("cmac")]
        CmacAes,
        /// <summary>
        /// AES-CCM
        /// </summary>
        [Description("aes-ccm")]
        AesCcm
    }
}