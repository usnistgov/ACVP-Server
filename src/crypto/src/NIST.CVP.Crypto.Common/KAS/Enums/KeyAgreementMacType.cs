using System.ComponentModel;
using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.KAS.Enums
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
        [EnumMember(Value = "hmac-sha2-224")]
        HmacSha2D224,
        /// <summary>
        /// HMAC - SHA2-256
        /// </summary>
        [EnumMember(Value = "hmac-sha2-256")]
        HmacSha2D256,
        /// <summary>
        /// HMAC - SHA2-384
        /// </summary>
        [EnumMember(Value = "hmac-sha2-384")]
        HmacSha2D384,
        /// <summary>
        /// HMAC - SHA2-512
        /// </summary>
        [EnumMember(Value = "hmac-sha2-512")]
        HmacSha2D512,
        /// <summary>
        /// CMAC-AES
        /// </summary>
        [EnumMember(Value = "cmac")]
        CmacAes,
        /// <summary>
        /// AES-CCM
        /// </summary>
        [EnumMember(Value = "aes-ccm")]
        AesCcm
    }
}