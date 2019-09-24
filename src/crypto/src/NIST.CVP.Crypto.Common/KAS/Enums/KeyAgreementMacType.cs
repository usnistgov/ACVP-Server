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
        /// HMAC - SHA2-512/224
        /// </summary>
        [EnumMember(Value = "hmac-sha2-512/224")]
        HmacSha2D512_T224,
        /// <summary>
        /// HMAC - SHA2-512/256
        /// </summary>
        [EnumMember(Value = "hmac-sha2-512/256")]
        HmacSha2D512_T256,
        /// <summary>
        /// HMAC - SHA3-224
        /// </summary>
        [EnumMember(Value = "hmac-sha3-224")]
        HmacSha3D224,
        /// <summary>
        /// HMAC - SHA3-256
        /// </summary>
        [EnumMember(Value = "hmac-sha3-256")]
        HmacSha3D256,
        /// <summary>
        /// HMAC - SHA3-384
        /// </summary>
        [EnumMember(Value = "hmac-sha3-384")]
        HmacSha3D384,
        /// <summary>
        /// HMAC - SHA3-512
        /// </summary>
        [EnumMember(Value = "hmac-sha3-512")]
        HmacSha3D512,
        /// <summary>
        /// CMAC-AES
        /// </summary>
        [EnumMember(Value = "cmac")]
        CmacAes,
        /// <summary>
        /// AES-CCM
        /// </summary>
        [EnumMember(Value = "aes-ccm")]
        AesCcm,
        /// <summary>
        /// KMAC-128
        /// </summary>
        [EnumMember(Value="kmac-128")]
        Kmac_128,
        /// <summary>
        /// KMAC-256
        /// </summary>
        [EnumMember(Value = "kmac-256")]
        Kmac_256
    }
}