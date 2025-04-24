using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Common
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
        [EnumMember(Value = "ACVP-AES-XTS-2.0")]
        AES_XTS_v2_0,
        [EnumMember(Value = "CMAC-AES-1.0")]
        CMAC_AES_v1_0,
        [EnumMember(Value = "Ascon-AEAD128-SP800-232")]
        ASCON_AEAD128_SP800_232,
        [EnumMember(Value = "Ascon-Hash256-SP800-232")]
        ASCON_Hash256_SP800_232,
        [EnumMember(Value = "Ascon-XOF128-SP800-232")]
        ASCON_XOF128_SP800_232,
        [EnumMember(Value = "Ascon-CXOF128-SP800-232")]
        ASCON_CXOF128_SP800_232,
        [EnumMember(Value = "CMAC-TDES-1.0")]
        CMAC_TDES_v1_0,
        [EnumMember(Value = "ConditioningComponent-AES-CBC-MAC-SP800-90B")]
        ConditioningComponent_CBC_MAC_90B,
        [EnumMember(Value = "ConditioningComponent-BlockCipher_DF-SP800-90B")]
        ConditioningComponent_BlockCipher_DF_90B,
        [EnumMember(Value = "ConditioningComponent-Hash_DF-SP800-90B")]
        ConditioningComponent_Hash_DF_90B,
        [EnumMember(Value = "cSHAKE-128-1.0")]
        cSHAKE_128_v1_0,
        [EnumMember(Value = "cSHAKE-256-1.0")]
        cSHAKE_256_v1_0,
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
        [EnumMember(Value = "ECDSA-KeyGen-Fips186-5")]
        ECDSA_KeyGen_Fips186_5,
        [EnumMember(Value = "ECDSA-KeyVer-1.0")]
        ECDSA_KeyVer_v1_0,
        [EnumMember(Value = "ECDSA-KeyVer-Fips186-5")]
        ECDSA_KeyVer_Fips186_5,
        [EnumMember(Value = "ECDSA-SigGen-1.0")]
        ECDSA_SigGen_v1_0,
        [EnumMember(Value = "ECDSA-SigGen-Fips186-5")]
        ECDSA_SigGen_Fips186_5,
        [EnumMember(Value = "ECDSA-SigVer-1.0")]
        ECDSA_SigVer_v1_0,
        [EnumMember(Value = "ECDSA-SigVer-Fips186-5")]
        ECDSA_SigVer_Fips186_5,
        [EnumMember(Value = "DetECDSA-SigGen-Fips186-5")]
        DetECDSA_SigGen_Fips186_5,
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
        //[EnumMember(Value = "HSS-KeyGen-1.0")]
        //HSS_KeyGen_v1_0,
        //[EnumMember(Value = "HSS-SigGen-1.0")]
        //HSS_SigGen_v1_0,
        //[EnumMember(Value = "HSS-SigVer-1.0")]
        //HSS_SigVer_v1_0,
        [EnumMember(Value = "KAS-ECC-1.0")]
        KAS_ECC_v1_0,
        [EnumMember(Value = "KAS-ECC-Component-1.0")]
        KAS_ECC_Component_v1_0,
        [EnumMember(Value = "KAS-ECC-CDH-Component-1.0")]
        KAS_EccComponent_v1_0,
        [EnumMember(Value = "KAS-ECC-CDH-Component-Sp800-56Ar3")]
        KAS_EccComponent_Sp800_56Ar3,
        [EnumMember(Value = "KAS-FFC-1.0")]
        KAS_FFC_v1_0,
        [EnumMember(Value = "KAS-FFC-Component-1.0")]
        KAS_FFC_Component_v1_0,
        [EnumMember(Value = "KAS-FFC-Sp800-56Ar3")]
        KAS_FFC_Sp800_56Ar3,
        [EnumMember(Value = "KAS-FFC-SSC-SP800-56Ar3")]
        KAS_FFC_SSC_Sp800_56Ar3,
        [EnumMember(Value = "KAS-ECC-Sp800-56Ar3")]
        KAS_ECC_Sp800_56Ar3,
        [EnumMember(Value = "KAS-ECC-SSC-SP800-56Ar3")]
        KAS_ECC_SSC_Sp800_56Ar3,
        [EnumMember(Value = "KAS-IFC-Sp800-56Br2")]
        KAS_IFC_Sp800_56Br2,
        [EnumMember(Value = "KAS-IFC-SSC-Sp800-56Br2")]
        KAS_IFC_SSC_Sp800_56Br2,
        [EnumMember(Value = "KAS-KC-Sp800-56")]
        KAS_KC_Sp800_56,
        [EnumMember(Value = "KDA-OneStep-Sp800-56Cr1")]
        KDA_OneStep_Sp800_56Cr1,
        [EnumMember(Value = "KDA-OneStep-Sp800-56Cr2")]
        KDA_OneStep_Sp800_56Cr2,
        [EnumMember(Value = "KDA-OneStepNoCounter-Sp800-56Cr2")]
        KDA_OneStepNoCounter_Sp800_56Cr2,
        [EnumMember(Value = "KDA-TwoStep-Sp800-56Cr1")]
        KDA_TwoStep_Sp800_56Cr1,
        [EnumMember(Value = "KDA-TwoStep-Sp800-56Cr2")]
        KDA_TwoStep_Sp800_56Cr2,
        [EnumMember(Value = "KDA-HKDF-Sp800-56Cr1")]
        KDA_HKDF_Sp800_56Cr1,
        [EnumMember(Value = "KDA-HKDF-Sp800-56Cr2")]
        KDA_HKDF_Sp800_56Cr2,
        [EnumMember(Value = "KTS-IFC-Sp800-56Br2")]
        KTS_IFC_Sp800_56Br2,
        [EnumMember(Value = "KDF-1.0")]
        KDF_v1_0,
        [EnumMember(Value = "KDF-KMAC-Sp800-108r1")]
        KDF_KMAC_Sp800_108r1,
        // proposed refactor but not done
        //[EnumMember(Value = "KDF-Counter-Sp800-108r1")]
        //KDF_Counter_Sp800_108r1,
        //[EnumMember(Value = "KDF-Feedback-Sp800-108r1")]
        //KDF_Feedback_Sp800_108r1,
        //[EnumMember(Value = "KDF-DoublePipeline-Sp800-108r1")]
        //KDF_DoublePipeline_Sp800_108r1,
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
        [EnumMember(Value = "KDF-Components-ANSIX9.42-1.0")]
        KDFComponents_ANSIX942_v1_0,
        [EnumMember(Value = "KMAC-128-1.0")]
        KMAC_128_v1_0,
        [EnumMember(Value = "KMAC-256-1.0")]
        KMAC_256_v1_0,
        [EnumMember(Value = "LMS-KeyGen-1.0")]
        LMS_KeyGen_v1_0,
        [EnumMember(Value = "LMS-SigGen-1.0")]
        LMS_SigGen_v1_0,
        [EnumMember(Value = "LMS-SigVer-1.0")]
        LMS_SigVer_v1_0,
        [EnumMember(Value = "ML-DSA-KeyGen-FIPS204")]
        ML_DSA_KeyGen_FIPS204,
        [EnumMember(Value = "ML-DSA-SigGen-FIPS204")]
        ML_DSA_SigGen_FIPS204,
        [EnumMember(Value = "ML-DSA-SigVer-FIPS204")]
        ML_DSA_SigVer_FIPS204,
        [EnumMember(Value = "ML-KEM-KeyGen-FIPS203")]
        ML_KEM_KeyGen_FIPS203,
        [EnumMember(Value = "ML-KEM-EncapDecap-FIPS203")]
        ML_KEM_EncapDecap_FIPS203,
        [EnumMember(Value = "ParallelHash-128-1.0")]
        ParallelHash_128_v1_0,
        [EnumMember(Value = "ParallelHash-256-1.0")]
        ParallelHash_256_v1_0,
        [EnumMember(Value = "PBKDF-1.0")]
        PBKDF_v1_0,
        [EnumMember(Value = "RSA-DecryptionPrimitive-1.0")]
        RSA_DecryptionPrimitive_v1_0,
        [EnumMember(Value = "RSA-DecryptionPrimitive-Sp800-56Br2")]
        RSA_DecryptionPrimitive_Sp800_56Br2,
        [EnumMember(Value = "RSA-KeyGen-Fips186-4")]
        RSA_KeyGen_Fips186_4,
        [EnumMember(Value = "RSA-KeyGen-Fips186-5")]
        RSA_KeyGen_Fips186_5,
        [EnumMember(Value = "RSA-SigGen-Fips186-4")]
        RSA_SigGen_Fips186_4,
        [EnumMember(Value = "RSA-SigGen-Fips186-5")]
        RSA_SigGen_Fips186_5,
        [EnumMember(Value = "RSA-SigVer-Fips186-2")]
        RSA_SigVer_Fips186_2,
        [EnumMember(Value = "RSA-SigVer-Fips186-4")]
        RSA_SigVer_Fips186_4,
        [EnumMember(Value = "RSA-SigVer-Fips186-5")]
        RSA_SigVer_Fips186_5,
        [EnumMember(Value = "RSA-SignaturePrimitive-1.0")]
        RSA_SignaturePrimitive_v1_0,
        [EnumMember(Value = "RSA-SignaturePrimitive-2.0")]
        RSA_SignaturePrimitive_v2_0,
        [EnumMember(Value = "SafePrimes-KeyGen-1.0")]
        SafePrimes_keyGen_v1_0,
        [EnumMember(Value = "SafePrimes-KeyVer-1.0")]
        SafePrimes_keyVer_v1_0,
        [EnumMember(Value = "SLH-DSA-KeyGen-FIPS205")]
        SLH_DSA_KeyGen_FIPS205,
        [EnumMember(Value = "SLH-DSA-SigGen-FIPS205")]
        SLH_DSA_SigGen_FIPS205,
        [EnumMember(Value = "SLH-DSA-SigVer-FIPS205")]
        SLH_DSA_SigVer_FIPS205,
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
        [EnumMember(Value = "SHA3-224-2.0")]
        SHA3_224_v2_0,
        [EnumMember(Value = "SHA3-256-2.0")]
        SHA3_256_v2_0,
        [EnumMember(Value = "SHA3-384-2.0")]
        SHA3_384_v2_0,
        [EnumMember(Value = "SHA3-512-2.0")]
        SHA3_512_v2_0,
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
        [EnumMember(Value = "TLS-v1.2-KDF-RFC7627")]
        Tls_v1_2_RFC7627,
        [EnumMember(Value = "TLS-v1.3-KDF-RFC8446")]
        Tls_v1_3_v1_0,
        [EnumMember(Value = "TupleHash-128-1.0")]
        TupleHash_128_v1_0,
        [EnumMember(Value = "TupleHash-256-1.0")]
        TupleHash_256_v1_0
    }
}
