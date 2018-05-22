using System.ComponentModel;
using System.Runtime.Serialization;

namespace NIST.CVP.Common
{
    /// <summary>
    /// Algorithm and Mode enum, algorithm and mode are 
    /// separated by a "-" or "_" in the Description/Value respectively.
    /// </summary>
    public enum AlgoMode
    {
        [EnumMember(Value = "AES-CBC")]
        AES_CBC,
        [EnumMember(Value = "AES-CCM")]
        AES_CCM,
        [EnumMember(Value = "AES-CFB1")]
        AES_CFB1,
        [EnumMember(Value = "AES-CFB8")]
        AES_CFB8,
        [EnumMember(Value = "AES-CFB128")]
        AES_CFB128,
        [EnumMember(Value = "AES-CTR")]
        AES_CTR,
        [EnumMember(Value = "AES-ECB")]
        AES_ECB,
        [EnumMember(Value = "AES-GCM")]
        AES_GCM,
        [EnumMember(Value = "AES-KW")]
        AES_KW,
        [EnumMember(Value = "AES-KWP")]
        AES_KWP,
        [EnumMember(Value = "AES-OFB")]
        AES_OFB,
        [EnumMember(Value = "AES-XPN")]
        AES_XPN,
        [EnumMember(Value = "AES-XTS")]
        AES_XTS,
        [EnumMember(Value = "CMAC-AES")]
        CMAC_AES,
        [EnumMember(Value = "CMAC-TDES")]
        CMAC_TDES,
        [EnumMember(Value = "ctrDRBG")]
        DRBG_CTR,
        [EnumMember(Value = "hashDRBG")]
        DRBG_Hash,
        [EnumMember(Value = "hmacDRBG")]
        DRBG_HMAC,
        [EnumMember(Value = "DSA-KeyGen")]
        DSA_KeyGen,
        [EnumMember(Value = "DSA-PQGGen")]
        DSA_PQGGen,
        [EnumMember(Value = "DSA-PQGVer")]
        DSA_PQGVer,
        [EnumMember(Value = "DSA-SigGen")]
        DSA_SigGen,
        [EnumMember(Value = "DSA-SigVer")]
        DSA_SigVer,
        [EnumMember(Value = "ECDSA-KeyGen")]
        ECDSA_KeyGen,
        [EnumMember(Value = "ECDSA-KeyVer")]
        ECDSA_KeyVer,
        [EnumMember(Value = "ECDSA-SigGen")]
        ECDSA_SigGen,
        [EnumMember(Value = "ECDSA-SigVer")]
        ECDSA_SigVer,
        [EnumMember(Value = "HMAC-SHA-1")]
        HMAC_SHA1,
        [EnumMember(Value = "HMAC-SHA2-224")]
        HMAC_SHA2_224,
        [EnumMember(Value = "HMAC-SHA2-256")]
        HMAC_SHA2_256,
        [EnumMember(Value = "HMAC-SHA2-384")]
        HMAC_SHA2_384,
        [EnumMember(Value = "HMAC-SHA2-512")]
        HMAC_SHA2_512,
        [EnumMember(Value = "HMAC-SHA2-512/224")]
        HMAC_SHA2_512_224,
        [EnumMember(Value = "HMAC-SHA2-512/256")]
        HMAC_SHA2_512_256,
        [EnumMember(Value = "HMAC-SHA3-224")]
        HMAC_SHA3_224,        
        [EnumMember(Value = "HMAC-SHA3-256")]
        HMAC_SHA3_256,        
        [EnumMember(Value = "HMAC-SHA3-384")]
        HMAC_SHA3_384,        
        [EnumMember(Value = "HMAC-SHA3-512")]
        HMAC_SHA3_512,
        [EnumMember(Value = "KAS-ECC")]
        KAS_ECC,
        [EnumMember(Value = "KAS-EccCDH-Component")]
        KAS_EccComponent,
        [EnumMember(Value = "KAS-FFC")]
        KAS_FFC,
        [EnumMember(Value = "KDF")]
        KDF,
        [EnumMember(Value = "KDF-Components-ANSIX9.63")]
        KDFComponents_ANSIX963,
        [EnumMember(Value = "KDF-Components-IKEv1")]
        KDFComponents_IKEv1,
        [EnumMember(Value = "KDF-Components-IKEv2")]
        KDFComponents_IKEv2,
        [EnumMember(Value = "KDF-Components-SNMP")]
        KDFComponents_SNMP,
        [EnumMember(Value = "KDF-Components-SRTP")]
        KDFComponents_SRTP,
        [EnumMember(Value = "KDF-Components-SSH")]
        KDFComponents_SSH,
        [EnumMember(Value = "KDF-Components-TLS")]
        KDFComponents_TLS,
        [EnumMember(Value = "RSA-DecryptionPrimitive")]
        RSA_DecryptionPrimitive,
        [EnumMember(Value = "RSA-KeyGen")]
        RSA_KeyGen,
        [EnumMember(Value = "RSA-SigGen")]
        RSA_SigGen,
        [EnumMember(Value = "RSA-SigVer")]
        RSA_SigVer,
        [EnumMember(Value = "RSA-LegacySigVer")]
        RSA_LegacySigVer,
        [EnumMember(Value = "RSA-SignaturePrimitive")]
        RSA_SignaturePrimitive,
        [EnumMember(Value = "SHA1")]
        SHA1,
        [EnumMember(Value = "SHA2")]
        SHA2,
        [EnumMember(Value = "SHA3")]
        SHA3,
        [EnumMember(Value = "SHAKE")]
        SHAKE,
        [EnumMember(Value = "TDES-CBC")]
        TDES_CBC,
        [EnumMember(Value = "TDES-CBCI")]
        TDES_CBCI,
        [EnumMember(Value = "TDES-CFB1")]
        TDES_CFB1,
        [EnumMember(Value = "TDES-CFB8")]
        TDES_CFB8,
        [EnumMember(Value = "TDES-CFB64")]
        TDES_CFB64,
        [EnumMember(Value = "TDES-CFBP1")]
        TDES_CFBP1,
        [EnumMember(Value = "TDES-CFBP8")]
        TDES_CFBP8,
        [EnumMember(Value = "TDES-CFBP64")]
        TDES_CFBP64,
        [EnumMember(Value = "TDES-CTR")]
        TDES_CTR,
        [EnumMember(Value = "TDES-ECB")]
        TDES_ECB,
        [EnumMember(Value = "TDES-KW")]
        TDES_KW,
        [EnumMember(Value = "TDES-OFB")]
        TDES_OFB,
        [EnumMember(Value = "TDES-OFBI")]
        TDES_OFBI
    }
}