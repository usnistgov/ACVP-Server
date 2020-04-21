using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.AES;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.Component;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.DRBG;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.DSA;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.ECDSA;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.HMAC;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.KAS;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.KDF;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.RSA;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.SHA_3;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.SHS;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.TDES;

namespace NIST.CVP.Libraries.Internal.LCAVPCore
{
	//Note - This class is unused, as each Evaluator explicitly news up the appropriate results extractor, instead of using this factory. That eliminates a cast in each evaluator, for better or worse
	public class AlgorithmResultsExtractorFactory
	{
		public IAlgorithmResultsExtractor GetExtractor(string algorithmName, string submissionPath)
		{
			switch (algorithmName)
			{
				case "AES-CBC": return new AES_CBC_ResultsExtractor(submissionPath);
				case "AES-CCM": return new AES_CCM_ResultsExtractor(submissionPath);
				case "AES-CFB1": return new AES_CFB1_ResultsExtractor(submissionPath);
				case "AES-CFB128": return new AES_CFB128_ResultsExtractor(submissionPath);
				case "AES-CFB8": return new AES_CFB8_ResultsExtractor(submissionPath);
				case "AES-CMAC": return new AES_CMAC_ResultsExtractor(submissionPath);
				case "AES-CTR": return new AES_CTR_ResultsExtractor(submissionPath);
				case "AES-ECB": return new AES_ECB_ResultsExtractor(submissionPath);
				case "AES-GCM": return new AES_GCM_ResultsExtractor(submissionPath);
				case "AES-GMAC": return new AES_GMAC_ResultsExtractor(submissionPath);
				case "AES-KW": return new AES_KW_ResultsExtractor(submissionPath);
				case "AES-KWP": return new AES_KWP_ResultsExtractor(submissionPath);
				case "AES-OFB": return new AES_OFB_ResultsExtractor(submissionPath);
				case "AES-XPN": return new AES_XPN_ResultsExtractor(submissionPath);
				case "AES-XTS": return new AES_XTS_ResultsExtractor(submissionPath);
				case "ANS 9.63": return new ANS_963_ResultsExtractor(submissionPath);
				//case "ANSI X9.31":
				//case "ANSI X9.62":
				//case "CCM":
				case "Component-KAS-ECC": return new Component_KAS_ECC_ResultsExtractor(submissionPath);
				case "Component-KAS-FFC": return new Component_KAS_FFC_ResultsExtractor(submissionPath);
				//case "counterMode": return new Counter_ResultsExtractor(submissionPath);
				case "CTR_DRBG": return new CTR_ResultsExtractor(submissionPath);
				//case "DES-CBC":
				//case "DES-CFB1":
				//case "DES-CFB64":
				//case "DES-CFB8":
				//case "DES-CTR":
				//case "DES-ECB":
				//case "DES-OFB":
				//case "doublePipelineIterationMode": return new DoublePipeline_ResultsExtractor(submissionPath);
				case "DSAKeyPair": return new DSAKeyPair_ResultsExtractor(submissionPath);
				case "DSAPQGGen": return new DSAPQGGen_ResultsExtractor(submissionPath);
				case "DSAPQGVer": return new DSAPQGVer_ResultsExtractor(submissionPath);
				case "DSASigGen": return new DSASigGen_ResultsExtractor(submissionPath);
				case "DSASigVer": return new DSASigVer_ResultsExtractor(submissionPath);
				//case "Dual_EC_DRBG":
				case "ECC CDH": return new ECC_CDH_ResultsExtractor(submissionPath);
				case "ECDSAKeyPair": return new ECDSAKeyPair_ResultsExtractor(submissionPath);
				case "ECDSAPKV": return new ECDSAPKV_ResultsExtractor(submissionPath);
				case "ECDSASigGen": return new ECDSASigGen_ResultsExtractor(submissionPath);
				case "ECDSASigVer": return new ECDSASigVer_ResultsExtractor(submissionPath);
				case "ECDSA SigGen Component": return new ECDSASigGenComponent_ResultsExtractor(submissionPath);
				//case "feedbackMode": return new Feedback_ResultsExtractor(submissionPath);
				//case "FIPS 186-2":
				//case "FIPS 186-2 General Purpose":
				case "Hash_Based DRBG": return new Hash_ResultsExtractor(submissionPath);
				case "HMAC_Based DRBG": return new HMAC_ResultsExtractor(submissionPath);
				case "HMAC-SHA-1": return new HMAC_SHA_1_ResultsExtractor(submissionPath);
				case "HMAC-SHA2-224": return new HMAC_SHA2_224_ResultsExtractor(submissionPath);
				case "HMAC-SHA2-256": return new HMAC_SHA2_256_ResultsExtractor(submissionPath);
				case "HMAC-SHA2-384": return new HMAC_SHA2_384_ResultsExtractor(submissionPath);
				case "HMAC-SHA2-512": return new HMAC_SHA2_512_ResultsExtractor(submissionPath);
				case "HMAC-SHA2-512/224": return new HMAC_SHA2_512224_ResultsExtractor(submissionPath);
				case "HMAC-SHA2-512/256": return new HMAC_SHA2_512256_ResultsExtractor(submissionPath);
				case "HMAC-SHA3-224": return new HMAC_SHA3_224_ResultsExtractor(submissionPath);
				case "HMAC-SHA3-256": return new HMAC_SHA3_256_ResultsExtractor(submissionPath);
				case "HMAC-SHA3-384": return new HMAC_SHA3_384_ResultsExtractor(submissionPath);
				case "HMAC-SHA3-512": return new HMAC_SHA3_512_ResultsExtractor(submissionPath);
				case "IKEv1": return new IKEv1_ResultsExtractor(submissionPath);
				case "IKEv2": return new IKEv2_ResultsExtractor(submissionPath);
				case "KAS ECC": return new KAS_ECC_ResultsExtractor(submissionPath);
				case "KAS FFC": return new KAS_FFC_ResultsExtractor(submissionPath);
				case "KDF": return new KDF_ResultsExtractor(submissionPath);
				case "RSAKeyGen": return new RSAKeyGen_ResultsExtractor(submissionPath);
				case "RSAKeyGen186-2": return new RSAKeyGen186_2_ResultsExtractor(submissionPath);
				case "RSALegacySigVer": return new RSALegacySigVer_ResultsExtractor(submissionPath);
				case "RSASigGen": return new RSASigGen_ResultsExtractor(submissionPath);
				case "RSASigGen186-2": return new RSASigGen186_2_ResultsExtractor(submissionPath);
				case "RSASigVer": return new RSASigVer_ResultsExtractor(submissionPath);
				case "RSADP": return new RSADP_ResultsExtractor(submissionPath);
				case "RSASP1": return new RSASP1_ResultsExtractor(submissionPath);
				case "SHA-1": return new SHA_1_ResultsExtractor(submissionPath);
				case "SHA-224": return new SHA_224_ResultsExtractor(submissionPath);
				case "SHA-256": return new SHA_256_ResultsExtractor(submissionPath);
				case "SHA3-224": return new SHA3_224_ResultsExtractor(submissionPath);
				case "SHA3-256": return new SHA3_256_ResultsExtractor(submissionPath);
				case "SHA3-384": return new SHA3_384_ResultsExtractor(submissionPath);
				case "SHA3-512": return new SHA3_512_ResultsExtractor(submissionPath);
				case "SHA-384": return new SHA_384_ResultsExtractor(submissionPath);
				case "SHA-512": return new SHA_512_ResultsExtractor(submissionPath);
				case "SHA-512/224": return new SHA_512224_ResultsExtractor(submissionPath);
				case "SHA-512/256": return new SHA_512256_ResultsExtractor(submissionPath);
				case "SHAKE-128": return new SHAKE_128_ResultsExtractor(submissionPath);
				case "SHAKE-256": return new SHAKE_256_ResultsExtractor(submissionPath);
				//case "SJ-CBC":
				//case "SJ-CFB1":
				//case "SJ-CFB16":
				//case "SJ-CFB32":
				//case "SJ-CFB64":
				//case "SJ-CFB8":
				//case "SJ-ECB":
				//case "SJ-OFB":
				case "SNMP": return new SNMP_ResultsExtractor(submissionPath);
				case "SRTP": return new SRTP_ResultsExtractor(submissionPath);
				case "SSH": return new SSH_ResultsExtractor(submissionPath);
				case "TDES-CBC": return new TDES_CBC_ResultsExtractor(submissionPath);
				case "TDES-CBC-I": return new TDES_CBCI_ResultsExtractor(submissionPath);
				case "TDES-CFB1": return new TDES_CFB1_ResultsExtractor(submissionPath);
				case "TDES-CFB64": return new TDES_CFB64_ResultsExtractor(submissionPath);
				case "TDES-CFB8": return new TDES_CFB8_ResultsExtractor(submissionPath);
				case "TDES-CFB-P1": return new TDES_CFBP1_ResultsExtractor(submissionPath);
				case "TDES-CFB-P64": return new TDES_CFBP64_ResultsExtractor(submissionPath);
				case "TDES-CFB-P8": return new TDES_CFBP8_ResultsExtractor(submissionPath);
				case "TDES-CMAC": return new TDES_CMAC_ResultsExtractor(submissionPath);
				case "TDES-CTR": return new TDES_CTR_ResultsExtractor(submissionPath);
				case "TDES-ECB": return new TDES_ECB_ResultsExtractor(submissionPath);
				case "TDES-KW": return new TDES_KW_ResultsExtractor(submissionPath);
				case "TDES-OFB": return new TDES_OFB_ResultsExtractor(submissionPath);
				case "TDES-OFB-I": return new TDES_OFBI_ResultsExtractor(submissionPath);
				case "TLS": return new TLS_ResultsExtractor(submissionPath);
				case "TPM": return new TPM_ResultsExtractor(submissionPath);
				default:
					return null;
			}
		}
	}
}