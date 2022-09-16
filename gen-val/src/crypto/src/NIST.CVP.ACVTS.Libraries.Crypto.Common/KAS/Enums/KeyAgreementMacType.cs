using System.ComponentModel;
using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums
{
    public enum KeyAgreementMacType
    {
        /// <summary>
        /// MAC not used
        /// </summary>
        [EnumMember(Value = "none")]
        None,
        /// <summary>
        /// HMAC - SHA-1
        /// </summary>
        [EnumMember(Value = "HMAC-SHA-1")]
        HmacSha1,
        /// <summary>
        /// HMAC - SHA2-224
        /// </summary>
        [EnumMember(Value = "HMAC-SHA2-224")]
        HmacSha2D224,
        /// <summary>
        /// HMAC - SHA2-256
        /// </summary>
        [EnumMember(Value = "HMAC-SHA2-256")]
        HmacSha2D256,
        /// <summary>
        /// HMAC - SHA2-384
        /// </summary>
        [EnumMember(Value = "HMAC-SHA2-384")]
        HmacSha2D384,
        /// <summary>
        /// HMAC - SHA2-512
        /// </summary>
        [EnumMember(Value = "HMAC-SHA2-512")]
        HmacSha2D512,
        /// <summary>
        /// HMAC - SHA2-512/224
        /// </summary>
        [EnumMember(Value = "HMAC-SHA2-512/224")]
        HmacSha2D512_T224,
        /// <summary>
        /// HMAC - SHA2-512/256
        /// </summary>
        [EnumMember(Value = "HMAC-SHA2-512/256")]
        HmacSha2D512_T256,
        /// <summary>
        /// HMAC - SHA3-224
        /// </summary>
        [EnumMember(Value = "HMAC-SHA3-224")]
        HmacSha3D224,
        /// <summary>
        /// HMAC - SHA3-256
        /// </summary>
        [EnumMember(Value = "HMAC-SHA3-256")]
        HmacSha3D256,
        /// <summary>
        /// HMAC - SHA3-384
        /// </summary>
        [EnumMember(Value = "HMAC-SHA3-384")]
        HmacSha3D384,
        /// <summary>
        /// HMAC - SHA3-512
        /// </summary>
        [EnumMember(Value = "HMAC-SHA3-512")]
        HmacSha3D512,
        /// <summary>
        /// CMAC-AES
        /// </summary>
        [EnumMember(Value = "CMAC")]
        CmacAes,
        /// <summary>
        /// AES-CCM
        /// </summary>
        [EnumMember(Value = "AES-CCM")]
        AesCcm,
        /// <summary>
        /// KMAC-128
        /// </summary>
        [EnumMember(Value = "KMAC-128")]
        Kmac_128,
        /// <summary>
        /// KMAC-256
        /// </summary>
        [EnumMember(Value = "KMAC-256")]
        Kmac_256
    }
}
