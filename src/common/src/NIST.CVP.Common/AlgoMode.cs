using System.Runtime.Serialization;

namespace NIST.CVP.Common
{
    /// <summary>
    /// Algorithm and Mode enum_v1_0, algorithm and mode are 
    /// separated by a "-" or "_" in the Description/Value respectively.
    /// </summary>
    public enum AlgoMode
    {
        [EnumMember(Value = "AES-CBC-1.0")]
        AES_CBC_v1_0,
        [EnumMember(Value = "AES-CBC-CTS-1.0")]
        AES_CBC_CTS_v1_0,
        [EnumMember(Value = "AES-CCM-1.0")]
        AES_CCM_v1_0,
        [EnumMember(Value = "AES-CFB1-1.0")]
        AES_CFB1_v1_0,
        [EnumMember(Value = "AES-CFB8-1.0")]
        AES_CFB8_v1_0,
        [EnumMember(Value = "AES-CFB128-1.0")]
        AES_CFB128_v1_0,
        [EnumMember(Value = "AES-CTR-1.0")]
        AES_CTR_v1_0,
        [EnumMember(Value = "AES-ECB-1.0")]
        AES_ECB_v1_0,
        [EnumMember(Value = "AES-GCM-1.0")]
        AES_GCM_v1_0,
        [EnumMember(Value = "AES-GCM-SIV-1.0")]
        AES_GCM_SIV_v1_0,
        [EnumMember(Value = "AES-KW-1.0")]
        AES_KW_v1_0,
        [EnumMember(Value = "AES-KWP-1.0")]
        AES_KWP_v1_0,
        [EnumMember(Value = "AES-OFB-1.0")]
        AES_OFB_v1_0,
        [EnumMember(Value = "AES-XPN-1.0")]
        AES_XPN_v1_0,
        [EnumMember(Value = "AES-XTS-1.0")]
        AES_XTS_v1_0,
        [EnumMember(Value = "CMAC-AES-1.0")]
        CMAC_AES_v1_0,
        [EnumMember(Value = "CMAC-TDES-1.0")]
        CMAC_TDES_v1_0,
        [EnumMember(Value = "CSHAKE-1.0")]
        CSHAKE_v1_0,
        [EnumMember(Value = "ctrDRBG-1.0")]
        DRBG_CTR_v1_0,
        [EnumMember(Value = "hashDRBG-1.0")]
        DRBG_Hash_v1_0,
        [EnumMember(Value = "hmacDRBG-1.0")]
        DRBG_HMAC_v1_0,
        [EnumMember(Value = "DSA-KeyGen-1.0")]
        DSA_KeyGen_v1_0,
        [EnumMember(Value = "DSA-PQGGen-1.0")]
        DSA_PQGGen_v1_0,
        [EnumMember(Value = "DSA-PQGVer-1.0")]
        DSA_PQGVer_v1_0,
        [EnumMember(Value = "DSA-SigGen-1.0")]
        DSA_SigGen_v1_0,
        [EnumMember(Value = "DSA-SigVer-1.0")]
        DSA_SigVer_v1_0,
        [EnumMember(Value = "ECDSA-KeyGen-1.0")]
        ECDSA_KeyGen_v1_0,
        [EnumMember(Value = "ECDSA-KeyVer-1.0")]
        ECDSA_KeyVer_v1_0,
        [EnumMember(Value = "ECDSA-SigGen-1.0")]
        ECDSA_SigGen_v1_0,
        [EnumMember(Value = "ECDSA-SigVer-1.0")]
        ECDSA_SigVer_v1_0,
        [EnumMember(Value = "EDDSA-KeyGen-1.0")]
        EDDSA_KeyGen_v1_0,
        [EnumMember(Value = "EDDSA-KeyVer-1.0")]
        EDDSA_KeyVer_v1_0,
        [EnumMember(Value = "EDDSA-SigGen-1.0")]
        EDDSA_SigGen_v1_0,
        [EnumMember(Value = "EDDSA-SigVer-1.0")]
        EDDSA_SigVer_v1_0,
        [EnumMember(Value = "HMAC-SHA-1-1.0")]
        HMAC_SHA1_v1_0,
        [EnumMember(Value = "HMAC-SHA2-224-1.0")]
        HMAC_SHA2_224_v1_0,
        [EnumMember(Value = "HMAC-SHA2-256-1.0")]
        HMAC_SHA2_256_v1_0,
        [EnumMember(Value = "HMAC-SHA2-384-1.0")]
        HMAC_SHA2_384_v1_0,
        [EnumMember(Value = "HMAC-SHA2-512-1.0")]
        HMAC_SHA2_512_v1_0,
        [EnumMember(Value = "HMAC-SHA2-512/224-1.0")]
        HMAC_SHA2_512_224_v1_0,
        [EnumMember(Value = "HMAC-SHA2-512/256-1.0")]
        HMAC_SHA2_512_256_v1_0,
        [EnumMember(Value = "HMAC-SHA3-224-1.0")]
        HMAC_SHA3_224_v1_0,        
        [EnumMember(Value = "HMAC-SHA3-256-1.0")]
        HMAC_SHA3_256_v1_0,        
        [EnumMember(Value = "HMAC-SHA3-384-1.0")]
        HMAC_SHA3_384_v1_0,        
        [EnumMember(Value = "HMAC-SHA3-512-1.0")]
        HMAC_SHA3_512_v1_0,
        [EnumMember(Value = "KAS-ECC-1.0")]
        KAS_ECC_v1_0,
        [EnumMember(Value = "KAS-EccCDH-Component-1.0")]
        KAS_EccComponent_v1_0,
        [EnumMember(Value = "KAS-FFC-1.0")]
        KAS_FFC_v1_0,
        [EnumMember(Value = "KDF-1.0")]
        KDF_v1_0,
        [EnumMember(Value = "KDF-Components-ANSIX9.63-1.0")]
        KDFComponents_ANSIX963_v1_0,
        [EnumMember(Value = "KDF-Components-IKEv1-1.0")]
        KDFComponents_IKEv1_v1_0,
        [EnumMember(Value = "KDF-Components-IKEv2-1.0")]
        KDFComponents_IKEv2_v1_0,
        [EnumMember(Value = "KDF-Components-SNMP-1.0")]
        KDFComponents_SNMP_v1_0,
        [EnumMember(Value = "KDF-Components-SRTP-1.0")]
        KDFComponents_SRTP_v1_0,
        [EnumMember(Value = "KDF-Components-SSH-1.0")]
        KDFComponents_SSH_v1_0,
        [EnumMember(Value = "KDF-Components-TLS-1.0")]
        KDFComponents_TLS_v1_0,
        [EnumMember(Value = "KDF-Components-TPM-1.0")]
        KDFComponents_TPM_v1_0,
        [EnumMember(Value = "KMAC-1.0")]
        KMAC_v1_0,
        [EnumMember(Value = "ParallelHash-1.0")]
        ParallelHash_v1_0,
        [EnumMember(Value = "RSA-DecryptionPrimitive-1.0")]
        RSA_DecryptionPrimitive_v1_0,
        [EnumMember(Value = "RSA-KeyGen-1.0")]
        RSA_KeyGen_v1_0,
        [EnumMember(Value = "RSA-SigGen-1.0")]
        RSA_SigGen_v1_0,
        [EnumMember(Value = "RSA-SigVer-1.0")]
        RSA_SigVer_v1_0,
        [EnumMember(Value = "RSA-LegacySigVer-1.0")]
        RSA_LegacySigVer_v1_0,
        [EnumMember(Value = "RSA-SignaturePrimitive-1.0")]
        RSA_SignaturePrimitive_v1_0,
        [EnumMember(Value = "SHA1-1.0")]
        SHA1_v1_0,
        [EnumMember(Value = "SHA2-1.0")]
        SHA2_v1_0,
        [EnumMember(Value = "SHA3-1.0")]
        SHA3_v1_0,
        [EnumMember(Value = "SHAKE-1.0")]
        SHAKE_v1_0,
        [EnumMember(Value = "TDES-CBC-1.0")]
        TDES_CBC_v1_0,
        [EnumMember(Value = "TDES-CBCI-1.0")]
        TDES_CBCI_v1_0,
        [EnumMember(Value = "TDES-CFB1-1.0")]
        TDES_CFB1_v1_0,
        [EnumMember(Value = "TDES-CFB8-1.0")]
        TDES_CFB8_v1_0,
        [EnumMember(Value = "TDES-CFB64-1.0")]
        TDES_CFB64_v1_0,
        [EnumMember(Value = "TDES-CFBP1-1.0")]
        TDES_CFBP1_v1_0,
        [EnumMember(Value = "TDES-CFBP8-1.0")]
        TDES_CFBP8_v1_0,
        [EnumMember(Value = "TDES-CFBP64-1.0")]
        TDES_CFBP64_v1_0,
        [EnumMember(Value = "TDES-CTR-1.0")]
        TDES_CTR_v1_0,
        [EnumMember(Value = "TDES-ECB-1.0")]
        TDES_ECB_v1_0,
        [EnumMember(Value = "TDES-KW-1.0")]
        TDES_KW_v1_0,
        [EnumMember(Value = "TDES-OFB-1.0")]
        TDES_OFB_v1_0,
        [EnumMember(Value = "TDES-OFBI-1.0")]
        TDES_OFBI_v1_0,
        [EnumMember(Value = "TupleHash-1.0")]
        TupleHash_v1_0
    }
}