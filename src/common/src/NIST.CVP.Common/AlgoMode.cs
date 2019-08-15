using System.Runtime.Serialization;

namespace NIST.CVP.Common
{
    /// <summary>
    /// Algorithm and Mode enum_v1_0, algorithm and mode are 
    /// separated by a "-" or "_" in the Description/Value respectively.
    /// </summary>
    public enum AlgoMode
    {
        [EnumMember(Value = "ACVP-AES-CBC-1.0")]
        AES_CBC_v1_0,
        [EnumMember(Value = "ACVP-AES-CBC-CS1-1.0")]
        AES_CBC_CS1_v1_0,
        [EnumMember(Value = "ACVP-AES-CBC-CS2-1.0")]
        AES_CBC_CS2_v1_0,
        [EnumMember(Value = "ACVP-AES-CBC-CS3-1.0")]
        AES_CBC_CS3_v1_0,
        [EnumMember(Value = "ACVP-AES-CCM-1.0")]
        AES_CCM_v1_0,
        [EnumMember(Value = "ACVP-AES-CFB1-1.0")]
        AES_CFB1_v1_0,
        [EnumMember(Value = "ACVP-AES-CFB8-1.0")]
        AES_CFB8_v1_0,
        [EnumMember(Value = "ACVP-AES-CFB128-1.0")]
        AES_CFB128_v1_0,
        [EnumMember(Value = "ACVP-AES-CTR-1.0")]
        AES_CTR_v1_0,
        [EnumMember(Value = "ACVP-AES-ECB-1.0")]
        AES_ECB_v1_0,
        [EnumMember(Value = "ACVP-AES-FF1-1.0")]
        AES_FF1_v1_0,
        [EnumMember(Value = "ACVP-AES-FF3-1-1.0")]
        AES_FF3_1_v1_0,
        [EnumMember(Value = "ACVP-AES-GCM-1.0")]
        AES_GCM_v1_0,
        [EnumMember(Value = "ACVP-AES-GMAC-1.0")]
        AES_GMAC_v1_0,
        [EnumMember(Value = "ACVP-AES-GCM-SIV-1.0")]
        AES_GCM_SIV_v1_0,
        [EnumMember(Value = "ACVP-AES-KW-1.0")]
        AES_KW_v1_0,
        [EnumMember(Value = "ACVP-AES-KWP-1.0")]
        AES_KWP_v1_0,
        [EnumMember(Value = "ACVP-AES-OFB-1.0")]
        AES_OFB_v1_0,
        [EnumMember(Value = "ACVP-AES-XPN-1.0")]
        AES_XPN_v1_0,
        [EnumMember(Value = "ACVP-AES-XTS-1.0")]
        AES_XTS_v1_0,
        [EnumMember(Value = "CMAC-AES-1.0")]
        CMAC_AES_v1_0,
        [EnumMember(Value = "CMAC-TDES-1.0")]
        CMAC_TDES_v1_0,
        [EnumMember(Value = "CSHAKE-128-1.0")]
        CSHAKE_128_v1_0,
        [EnumMember(Value = "CSHAKE-256-1.0")]
        CSHAKE_256_v1_0,
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
        [EnumMember(Value = "KAS-ECC-Component-1.0")]
        KAS_ECC_Component_v1_0,
        [EnumMember(Value = "KAS-Ecc-CDH-Component-1.0")]
        KAS_EccComponent_v1_0,
        [EnumMember(Value = "KAS-FFC-1.0")]
        KAS_FFC_v1_0,
        [EnumMember(Value = "KAS-FFC-Component-1.0")]
        KAS_FFC_Component_v1_0,
        [EnumMember(Value = "KAS-IFC-1.0")]
        KAS_IFC_v1_0,
        [EnumMember(Value = "KTS-IFC-1.0")]
        KTS_IFC_v1_0,
        [EnumMember(Value = "KDF-1.0")]
        KDF_v1_0,
        [EnumMember(Value = "KDF-Components-ANSIX9.63-1.0")]
        KDFComponents_ANSIX963_v1_0,
        [EnumMember(Value = "KDF-Components-IKEv1-1.0")]
        KDFComponents_IKEv1_v1_0,
        [EnumMember(Value = "KDF-Components-IKEv2-1.0")]
        KDFComponents_IKEv2_v1_0,
        [EnumMember(Value = "KDF-Components-PBKDF-1.0")]
        KDFComponents_PBKDF_v1_0,
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
        [EnumMember(Value = "KDF-Components-ANSIX9.42-1.0")]
        KDFComponents_ANSIX942_v1_0,
        [EnumMember(Value = "KMAC-128-1.0")]
        KMAC_128_v1_0,
        [EnumMember(Value = "KMAC-256-1.0")]
        KMAC_256_v1_0,
        [EnumMember(Value = "ParallelHash-128-1.0")]
        ParallelHash_128_v1_0,
        [EnumMember(Value = "ParallelHash-256-1.0")]
        ParallelHash_256_v1_0,
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
        [EnumMember(Value = "SHA-1-1.0")]
        SHA_1_v1_0,
        [EnumMember(Value = "SHA2-224-1.0")]
        SHA2_224_v1_0,
        [EnumMember(Value = "SHA2-256-1.0")]
        SHA2_256_v1_0,
        [EnumMember(Value = "SHA2-384-1.0")]
        SHA2_384_v1_0,
        [EnumMember(Value = "SHA2-512-1.0")]
        SHA2_512_v1_0,
        [EnumMember(Value = "SHA2-512/224-1.0")]
        SHA2_512_224_v1_0,
        [EnumMember(Value = "SHA2-512/256-1.0")]
        SHA2_512_256_v1_0,
        [EnumMember(Value = "SHA3-224-1.0")]
        SHA3_224_v1_0,
        [EnumMember(Value = "SHA3-256-1.0")]
        SHA3_256_v1_0,
        [EnumMember(Value = "SHA3-384-1.0")]
        SHA3_384_v1_0,
        [EnumMember(Value = "SHA3-512-1.0")]
        SHA3_512_v1_0,
        [EnumMember(Value = "SHAKE-128-1.0")]
        SHAKE_128_v1_0,
        [EnumMember(Value = "SHAKE-256-1.0")]
        SHAKE_256_v1_0,
        [EnumMember(Value = "ACVP-TDES-CBC-1.0")]
        TDES_CBC_v1_0,
        [EnumMember(Value = "ACVP-TDES-CBCI-1.0")]
        TDES_CBCI_v1_0,
        [EnumMember(Value = "ACVP-TDES-CFB1-1.0")]
        TDES_CFB1_v1_0,
        [EnumMember(Value = "ACVP-TDES-CFB8-1.0")]
        TDES_CFB8_v1_0,
        [EnumMember(Value = "ACVP-TDES-CFB64-1.0")]
        TDES_CFB64_v1_0,
        [EnumMember(Value = "ACVP-TDES-CFBP1-1.0")]
        TDES_CFBP1_v1_0,
        [EnumMember(Value = "ACVP-TDES-CFBP8-1.0")]
        TDES_CFBP8_v1_0,
        [EnumMember(Value = "ACVP-TDES-CFBP64-1.0")]
        TDES_CFBP64_v1_0,
        [EnumMember(Value = "ACVP-TDES-CTR-1.0")]
        TDES_CTR_v1_0,
        [EnumMember(Value = "ACVP-TDES-ECB-1.0")]
        TDES_ECB_v1_0,
        [EnumMember(Value = "ACVP-TDES-KW-1.0")]
        TDES_KW_v1_0,
        [EnumMember(Value = "ACVP-TDES-OFB-1.0")]
        TDES_OFB_v1_0,
        [EnumMember(Value = "ACVP-TDES-OFBI-1.0")]
        TDES_OFBI_v1_0,
        [EnumMember(Value = "TupleHash-128-1.0")]
        TupleHash_128_v1_0,
        [EnumMember(Value = "TupleHash-256-1.0")]
        TupleHash_256_v1_0
    }
}