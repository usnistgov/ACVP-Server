using LCAVPCore.AlgorithmEvaluators;
using LCAVPCore.AlgorithmEvaluators.AES;
using LCAVPCore.AlgorithmEvaluators.Component;
using LCAVPCore.AlgorithmEvaluators.DRBG;
using LCAVPCore.AlgorithmEvaluators.DSA;
using LCAVPCore.AlgorithmEvaluators.ECDSA;
using LCAVPCore.AlgorithmEvaluators.HMAC;
using LCAVPCore.AlgorithmEvaluators.KDF;
using LCAVPCore.AlgorithmEvaluators.RSA;
using LCAVPCore.AlgorithmEvaluators.SHA_3;
using LCAVPCore.AlgorithmEvaluators.SHS;
using LCAVPCore.AlgorithmEvaluators.TDES;
using LCAVPCore.AlgorithmEvaluators.KAS;

namespace LCAVPCore
{
	public class AlgorithmEvaluatorFactory : IAlgorithmEvaluatorFactory
	{
		public IAlgorithmEvaluator GetEvaluator(InfAlgorithm algorithm, string submissionPath)
		{
			switch (algorithm.AlgorithmName)
			{
				case "AES-CBC": return new AES_CBC_Evaluator(algorithm.Options, submissionPath);
				case "AES-CCM": return new AES_CCM_Evaluator(algorithm.Options, submissionPath);
				case "AES-CFB1": return new AES_CFB1_Evaluator(algorithm.Options, submissionPath);
				case "AES-CFB128": return new AES_CFB128_Evaluator(algorithm.Options, submissionPath);
				case "AES-CFB8": return new AES_CFB8_Evaluator(algorithm.Options, submissionPath);
				case "AES-CMAC": return new AES_CMAC_Evaluator(algorithm.Options, submissionPath);
				case "AES-CTR": return new AES_CTR_Evaluator(algorithm.Options, submissionPath);
				case "AES-ECB": return new AES_ECB_Evaluator(algorithm.Options, submissionPath);
				case "AES-GCM": return new AES_GCM_Evaluator(algorithm.Options, submissionPath);
				case "AES-GMAC": return new AES_GMAC_Evaluator(algorithm.Options, submissionPath);
				case "AES-KW": return new AES_KW_Evaluator(algorithm.Options, submissionPath);
				case "AES-KWP": return new AES_KWP_Evaluator(algorithm.Options, submissionPath);
				case "AES-OFB": return new AES_OFB_Evaluator(algorithm.Options, submissionPath);
				case "AES-XPN": return new AES_XPN_Evaluator(algorithm.Options, submissionPath);
				case "AES-XTS": return new AES_XTS_Evaluator(algorithm.Options, submissionPath);
				case "ANS 9.63": return new ANS_963_Evaluator(algorithm.Options, submissionPath);
				//case "ANSI X9.31":
				//case "ANSI X9.62":
				//case "CCM":
				case "Component-KAS-ECC": return new Component_KAS_ECC_Evaluator(algorithm.Options, submissionPath);
				case "Component-KAS-FFC": return new Component_KAS_FFC_Evaluator(algorithm.Options, submissionPath);
				//case "counterMode": return new Counter_Evaluator(algorithm.Options, submissionPath);
				case "CTR_DRBG": return new CTR_Evaluator(algorithm.Options, submissionPath);
				//case "DES-CBC":
				//case "DES-CFB1":
				//case "DES-CFB64":
				//case "DES-CFB8":
				//case "DES-CTR":
				//case "DES-ECB":
				//case "DES-OFB":
				//case "doublePipelineIterationMode": return new DoublePipeline_Evaluator(algorithm.Options, submissionPath);
				case "DSAKeyPair": return new DSAKeyPair_Evaluator(algorithm.Options, submissionPath);
				case "DSAPQGGen": return new DSAPQGGen_Evaluator(algorithm.Options, submissionPath);
				case "DSAPQGVer": return new DSAPQGVer_Evaluator(algorithm.Options, submissionPath);
				case "DSASigGen": return new DSASigGen_Evaluator(algorithm.Options, submissionPath);
				case "DSASigVer": return new DSASigVer_Evaluator(algorithm.Options, submissionPath);
				//case "Dual_EC_DRBG":
				case "ECC CDH": return new ECC_CDH_Evaluator(algorithm.Options, submissionPath);
				case "ECDSAKeyPair": return new ECDSAKeyPair_Evaluator(algorithm.Options, submissionPath);
				case "ECDSAPKV": return new ECDSAPKV_Evaluator(algorithm.Options, submissionPath);
				case "ECDSASigGen": return new ECDSASigGen_Evaluator(algorithm.Options, submissionPath);
				case "ECDSASigVer": return new ECDSASigVer_Evaluator(algorithm.Options, submissionPath);
				case "ECDSA SigGen Component": return new ECDSASigGenComponent_Evaluator(algorithm.Options, submissionPath);
				//case "feedbackMode": return new Feedback_Evaluator(algorithm.Options, submissionPath);
				//case "FIPS 186-2":
				//case "FIPS 186-2 General Purpose":
				case "Hash_Based DRBG": return new Hash_Evaluator(algorithm.Options, submissionPath);
				case "HMAC_Based DRBG": return new HMAC_Evaluator(algorithm.Options, submissionPath);
				case "HMAC-SHA-1": return new HMAC_SHA_1_Evaluator(algorithm.Options, submissionPath);
				case "HMAC-SHA2-224": return new HMAC_SHA2_224_Evaluator(algorithm.Options, submissionPath);
				case "HMAC-SHA2-256": return new HMAC_SHA2_256_Evaluator(algorithm.Options, submissionPath);
				case "HMAC-SHA2-384": return new HMAC_SHA2_384_Evaluator(algorithm.Options, submissionPath);
				case "HMAC-SHA2-512": return new HMAC_SHA2_512_Evaluator(algorithm.Options, submissionPath);
				case "HMAC-SHA2-512/224": return new HMAC_SHA2_512224_Evaluator(algorithm.Options, submissionPath);
				case "HMAC-SHA2-512/256": return new HMAC_SHA2_512256_Evaluator(algorithm.Options, submissionPath);
				case "HMAC-SHA3-224": return new HMAC_SHA3_224_Evaluator(algorithm.Options, submissionPath);
				case "HMAC-SHA3-256": return new HMAC_SHA3_256_Evaluator(algorithm.Options, submissionPath);
				case "HMAC-SHA3-384": return new HMAC_SHA3_384_Evaluator(algorithm.Options, submissionPath);
				case "HMAC-SHA3-512": return new HMAC_SHA3_512_Evaluator(algorithm.Options, submissionPath);
				case "IKEv1": return new IKEv1_Evaluator(algorithm.Options, submissionPath);
				case "IKEv2": return new IKEv2_Evaluator(algorithm.Options, submissionPath);
				case "KAS ECC": return new KAS_ECC_Evaluator(algorithm.Options, submissionPath);
				case "KAS FFC": return new KAS_FFC_Evaluator(algorithm.Options, submissionPath);
				case "KDF": return new KDF_Evaluator(algorithm.Options, submissionPath);
				case "RSAKeyGen": return new RSAKeyGen_Evaluator(algorithm.Options, submissionPath);
				case "RSAKeyGen186-2": return new RSAKeyGen186_2_Evaluator(algorithm.Options, submissionPath);
				case "RSALegacySigVer": return new RSALegacySigVer_Evaluator(algorithm.Options, submissionPath);
				case "RSASigGen": return new RSASigGen_Evaluator(algorithm.Options, submissionPath);
				case "RSASigGen186-2": return new RSASigGen186_2_Evaluator(algorithm.Options, submissionPath);
				case "RSASigVer": return new RSASigVer_Evaluator(algorithm.Options, submissionPath);
				case "RSADP": return new RSADP_Evaluator(algorithm.Options, submissionPath);
				case "RSASP1": return new RSASP1_Evaluator(algorithm.Options, submissionPath);
				case "SHA-1": return new SHA_1_Evaluator(algorithm.Options, submissionPath);
				case "SHA-224": return new SHA_224_Evaluator(algorithm.Options, submissionPath);
				case "SHA-256": return new SHA_256_Evaluator(algorithm.Options, submissionPath);
				case "SHA3-224": return new SHA3_224_Evaluator(algorithm.Options, submissionPath);
				case "SHA3-256": return new SHA3_256_Evaluator(algorithm.Options, submissionPath);
				case "SHA3-384": return new SHA3_384_Evaluator(algorithm.Options, submissionPath);
				case "SHA3-512": return new SHA3_512_Evaluator(algorithm.Options, submissionPath);
				case "SHA-384": return new SHA_384_Evaluator(algorithm.Options, submissionPath);
				case "SHA-512": return new SHA_512_Evaluator(algorithm.Options, submissionPath);
				case "SHA-512/224": return new SHA_512224_Evaluator(algorithm.Options, submissionPath);
				case "SHA-512/256": return new SHA_512256_Evaluator(algorithm.Options, submissionPath);
				case "SHAKE-128": return new SHAKE_128_Evaluator(algorithm.Options, submissionPath);
				case "SHAKE-256": return new SHAKE_256_Evaluator(algorithm.Options, submissionPath);
				//case "SJ-CBC":
				//case "SJ-CFB1":
				//case "SJ-CFB16":
				//case "SJ-CFB32":
				//case "SJ-CFB64":
				//case "SJ-CFB8":
				//case "SJ-ECB":
				//case "SJ-OFB":
				case "SNMP": return new SNMP_Evaluator(algorithm.Options, submissionPath);
				case "SRTP": return new SRTP_Evaluator(algorithm.Options, submissionPath);
				case "SSH": return new SSH_Evaluator(algorithm.Options, submissionPath);
				case "TDES-CBC": return new TDES_CBC_Evaluator(algorithm.Options, submissionPath);
				case "TDES-CBC-I": return new TDES_CBCI_Evaluator(algorithm.Options, submissionPath);
				case "TDES-CFB1": return new TDES_CFB1_Evaluator(algorithm.Options, submissionPath);
				case "TDES-CFB64": return new TDES_CFB64_Evaluator(algorithm.Options, submissionPath);
				case "TDES-CFB8": return new TDES_CFB8_Evaluator(algorithm.Options, submissionPath);
				case "TDES-CFB-P1": return new TDES_CFBP1_Evaluator(algorithm.Options, submissionPath);
				case "TDES-CFB-P64": return new TDES_CFBP64_Evaluator(algorithm.Options, submissionPath);
				case "TDES-CFB-P8": return new TDES_CFBP8_Evaluator(algorithm.Options, submissionPath);
				case "TDES-CMAC": return new TDES_CMAC_Evaluator(algorithm.Options, submissionPath);
				case "TDES-CTR": return new TDES_CTR_Evaluator(algorithm.Options, submissionPath);
				case "TDES-ECB": return new TDES_ECB_Evaluator(algorithm.Options, submissionPath);
				case "TDES-KW": return new TDES_KW_Evaluator(algorithm.Options, submissionPath);
				case "TDES-OFB": return new TDES_OFB_Evaluator(algorithm.Options, submissionPath);
				case "TDES-OFB-I": return new TDES_OFBI_Evaluator(algorithm.Options, submissionPath);
				case "TLS": return new TLS_Evaluator(algorithm.Options, submissionPath);
				case "TPM": return new TPM_Evaluator(algorithm.Options, submissionPath);
				default:
					return null;
			}
		}
	}
}