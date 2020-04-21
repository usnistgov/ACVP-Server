using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmChunkParsers
{
	public class KDF_800_135ChunkParser : AlgorithmChunkParserBase, IAlgorithmChunkParser
	{
		public KDF_800_135ChunkParser(InfFileSection chunk) : base(chunk)
		{
		}

		public List<InfAlgorithm> Parse()
		{
			//Read through the chunk, build up algorithms
			List<InfAlgorithm> algorithms = new List<InfAlgorithm>();

			algorithms.Add(ParseAlgorithmFromMode("IKEv1", "KDF_800_135_IKEv1", new List<string> {	"KDF_800_135_Prerequisite_SHA",
																									"KDF_800_135_Prerequisite_HMAC",
																									"KDF_800_135_IKEv1_DigitalSignatureAuthentication",
																									"KDF_800_135_IKEv1_PublicKeyEncryptionAuthentication",
																									"KDF_800_135_IKEv1_PreSharedKeyAuthentication",
																									"KDF_800_135_IKEv1_use_shared_secret_length0",
																									"KDF_800_135_IKEv1_shared_secret_length0",
																									"KDF_800_135_IKEv1_0_SHA_1",
																									"KDF_800_135_IKEv1_0_SHA_224",
																									"KDF_800_135_IKEv1_0_SHA_256",
																									"KDF_800_135_IKEv1_0_SHA_384",
																									"KDF_800_135_IKEv1_0_SHA_512",
																									"KDF_800_135_IKEv1_use_shared_secret_length1",
																									"KDF_800_135_IKEv1_shared_secret_length1",
																									"KDF_800_135_IKEv1_1_SHA_1",
																									"KDF_800_135_IKEv1_1_SHA_224",
																									"KDF_800_135_IKEv1_1_SHA_256",
																									"KDF_800_135_IKEv1_1_SHA_384",
																									"KDF_800_135_IKEv1_1_SHA_512",
																									"KDF_800_135_IKEv1_use_shared_secret_length2",
																									"KDF_800_135_IKEv1_shared_secret_length2",
																									"KDF_800_135_IKEv1_2_SHA_1",
																									"KDF_800_135_IKEv1_2_SHA_224",
																									"KDF_800_135_IKEv1_2_SHA_256",
																									"KDF_800_135_IKEv1_2_SHA_384",
																									"KDF_800_135_IKEv1_2_SHA_512",
																									"KDF_800_135_IKEv1_Ni(min)",
																									"KDF_800_135_IKEv1_Ni(max)",
																									"KDF_800_135_IKEv1_Nr(min)",
																									"KDF_800_135_IKEv1_Nr(max)",
																									"KDF_800_135_IKEv1_preshared_key(min)",
																									"KDF_800_135_IKEv1_preshared_key(max)"
																									}));

			algorithms.Add(ParseAlgorithmFromMode("IKEv2", "KDF_800_135_IKEv2", new List<string> {  "KDF_800_135_Prerequisite_SHA",
																									"KDF_800_135_Prerequisite_HMAC",
																									"KDF_800_135_IKEv2_use_shared_secret_length0",
																									"KDF_800_135_IKEv2_shared_secret_length0",
																									"KDF_800_135_IKEv2_0_SHA_1",
																									"KDF_800_135_IKEv2_0_SHA_224",
																									"KDF_800_135_IKEv2_0_SHA_256",
																									"KDF_800_135_IKEv2_0_SHA_384",
																									"KDF_800_135_IKEv2_0_SHA_512",
																									"KDF_800_135_IKEv2_use_shared_secret_length1",
																									"KDF_800_135_IKEv2_shared_secret_length1",
																									"KDF_800_135_IKEv2_1_SHA_1",
																									"KDF_800_135_IKEv2_1_SHA_224",
																									"KDF_800_135_IKEv2_1_SHA_256",
																									"KDF_800_135_IKEv2_1_SHA_384",
																									"KDF_800_135_IKEv2_1_SHA_512",
																									"KDF_800_135_IKEv2_use_shared_secret_length2",
																									"KDF_800_135_IKEv2_shared_secret_length2",
																									"KDF_800_135_IKEv2_2_SHA_1",
																									"KDF_800_135_IKEv2_2_SHA_224",
																									"KDF_800_135_IKEv2_2_SHA_256",
																									"KDF_800_135_IKEv2_2_SHA_384",
																									"KDF_800_135_IKEv2_2_SHA_512",
																									"KDF_800_135_IKEv2_Ni(min)",
																									"KDF_800_135_IKEv2_Ni(max)",
																									"KDF_800_135_IKEv2_Nr(min)",
																									"KDF_800_135_IKEv2_Nr(max)",
																									"KDF_800_135_IKEv2_DKM(min)",
																									"KDF_800_135_IKEv2_DKM(max)",
																									"KDF_800_135_IKEv2_DKM_ChildSA(min)",
																									"KDF_800_135_IKEv2_DKM_ChildSA(max)"
																									}));


			algorithms.Add(ParseAlgorithmFromMode("TLS", "KDF_800_135_TLS", new List<string> {	"KDF_800_135_Prerequisite_SHA",
																								"KDF_800_135_Prerequisite_HMAC",
																								"KDF_800_135_TLS10_11",
																								"KDF_800_135_TLS10_11_pre_master_secret_len",
																								"KDF_800_135_TLS10_11_key_block_len",
																								"KDF_800_135_TLS12",
																								"KDF_800_135_TLS12_SHA_256",
																								"KDF_800_135_TLS12_SHA_384",
																								"KDF_800_135_TLS12_SHA_512",
																								"KDF_800_135_TLS12_pre_master_secret_len",
																								"KDF_800_135_TLS12_key_block_len"
																								}));

			algorithms.Add(ParseAlgorithmFromMode("ANS 9.63", "KDF_800_135_ANSX963_2001", new List<string> {	"KDF_800_135_Prerequisite_SHA",
																												"KDF_800_135_ANSX963_2001_Z_length1",
																												"KDF_800_135_ANSX963_2001_OtherInfo_length1",
																												"KDF_800_135_ANSX963_2001_keydata_length1",
																												"KDF_800_135_ANSX963_2001_Z_length2",
																												"KDF_800_135_ANSX963_2001_OtherInfo_length2",
																												"KDF_800_135_ANSX963_2001_keydata_length2",
																												"KDF_800_135_ANSX963_2001_SHA_1",
																												"KDF_800_135_ANSX963_2001_SHA_224",
																												"KDF_800_135_ANSX963_2001_SHA_256",
																												"KDF_800_135_ANSX963_2001_SHA_384",
																												"KDF_800_135_ANSX963_2001_SHA_512"
																											}));

			algorithms.Add(ParseAlgorithmFromMode("SSH", "KDF_800_135_SSH", new List<string> {	"KDF_800_135_Prerequisite_SHA",
																								"KDF_800_135_SSH_K_length1",
																								"KDF_800_135_SSH_K_length2",
																								"KDF_800_135_SSH_TDES",
																								"KDF_800_135_SSH_AES_128",
																								"KDF_800_135_SSH_AES_192",
																								"KDF_800_135_SSH_AES_256",
																								"KDF_800_135_SSH_SHA_1",
																								"KDF_800_135_SSH_SHA_224",
																								"KDF_800_135_SSH_SHA_256",
																								"KDF_800_135_SSH_SHA_384",
																								"KDF_800_135_SSH_SHA_512"
																								}));

			algorithms.Add(ParseAlgorithmFromMode("SRTP", "KDF_800_135_SRTP", new List<string> {	"KDF_800_135_Prerequisite_AES",
																									"KDF_800_135_SRTP_AES_128",
																									"KDF_800_135_SRTP_AES_192",
																									"KDF_800_135_SRTP_AES_256",
																									"KDF_800_135_SRTP_KDROption",
																									"KDF_800_135_SRTP_KDR_0",
																									"KDF_800_135_SRTP_KDR_1",
																									"KDF_800_135_SRTP_KDR_2^1",
																									"KDF_800_135_SRTP_KDR_2^2",
																									"KDF_800_135_SRTP_KDR_2^3",
																									"KDF_800_135_SRTP_KDR_2^4",
																									"KDF_800_135_SRTP_KDR_2^5",
																									"KDF_800_135_SRTP_KDR_2^6",
																									"KDF_800_135_SRTP_KDR_2^7",
																									"KDF_800_135_SRTP_KDR_2^8",
																									"KDF_800_135_SRTP_KDR_2^9",
																									"KDF_800_135_SRTP_KDR_2^10",
																									"KDF_800_135_SRTP_KDR_2^11",
																									"KDF_800_135_SRTP_KDR_2^12",
																									"KDF_800_135_SRTP_KDR_2^13",
																									"KDF_800_135_SRTP_KDR_2^14",
																									"KDF_800_135_SRTP_KDR_2^15",
																									"KDF_800_135_SRTP_KDR_2^16",
																									"KDF_800_135_SRTP_KDR_2^17",
																									"KDF_800_135_SRTP_KDR_2^18",
																									"KDF_800_135_SRTP_KDR_2^19",
																									"KDF_800_135_SRTP_KDR_2^20",
																									"KDF_800_135_SRTP_KDR_2^21",
																									"KDF_800_135_SRTP_KDR_2^22",
																									"KDF_800_135_SRTP_KDR_2^23",
																									"KDF_800_135_SRTP_KDR_2^24"
																									}));

			algorithms.Add(ParseAlgorithmFromMode("SNMP", "KDF_800_135_SNMP", new List<string> {	"KDF_800_135_Prerequisite_SHA",
																									"KDF_800_135_snmpEngineId[0]",
																									"KDF_800_135_snmpEngineId[1]",
																									"KDF_800_135_snmpPasswordLength[0]",
																									"KDF_800_135_snmpPasswordLength[1]"
																								}));

			algorithms.Add(ParseAlgorithmFromMode("TPM", "KDF_800_135_TPM", new List<string> {  "KDF_800_135_Prerequisite_SHA",
																								"KDF_800_135_Prerequisite_HMAC",
																								}));

			//Since we might have added nulls to collection, for modes that were not "selected", filter them out before returning
			return algorithms.Where(x => x != null).ToList();
		}
	}
}