namespace NIST.CVP.Crypto.KAS.Enums
{
    public enum KeyAgreementMacType
    {
        /// <summary>
        /// HMAC - SHA2-224
        /// </summary>
        HmacSha2D224,
        /// <summary>
        /// HMAC - SHA2-256
        /// </summary>
        HmacSha2D256,
        /// <summary>
        /// HMAC - SHA2-384
        /// </summary>
        HmacSha2D384,
        /// <summary>
        /// HMAC - SHA2-512
        /// </summary>
        HmacSha2D512,
        /// <summary>
        /// CMAC-AES
        /// </summary>
        CmacAes,
        /// <summary>
        /// AES-CCM
        /// </summary>
        AesCcm
    }
}