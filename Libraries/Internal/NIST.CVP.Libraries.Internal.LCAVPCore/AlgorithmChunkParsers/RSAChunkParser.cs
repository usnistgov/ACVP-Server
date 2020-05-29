using System.Collections.Generic;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmChunkParsers
{
	public class RSAChunkParser : AlgorithmChunkParserBase, IAlgorithmChunkParser
	{
		public RSAChunkParser(InfFileSection chunk) : base(chunk)
		{
		}

		public List<InfAlgorithm> Parse()
		{
			//RSA is a single algorithm, so just need to convert the format
			//return new List<InfAlgorithm> { new InfAlgorithm("RSA", Chunk.KeyValuePairs) };

			//Read through the chunk, build up algorithms
			List<InfAlgorithm> algorithms = new List<InfAlgorithm>();
			InfAlgorithm algo;

			//186-2 KeyGen
			algo = ParseAlgorithmFromMode("RSAKeyGen186-2", "REVALONLY_FIPS186_2KeyGen", new List<string> { "REVALONLY_FIPS186_2KeyGen_Mod1024",
																											"REVALONLY_FIPS186_2KeyGen_Mod1536",
																											"REVALONLY_FIPS186_2KeyGen_Mod2048",
																											"REVALONLY_FIPS186_2KeyGen_Mod3072",
																											"REVALONLY_FIPS186_2KeyGen_Mod4096",
																											"REVALONLY_FIPS186_2KeyGen_E3",
																											"REVALONLY_FIPS186_2KeyGen_E17",
																											"REVALONLY_FIPS186_2KeyGen_E65537",
																											"FIPS186_3_RSA_Prerequisite_SHA_1",
																											"FIPS186_3_RSA_Prerequisite_SHA_2",
																											"FIPS186_3_RSA_Prerequisite_SHA_3",
																											"FIPS186_3_RSA_Prerequisite_DRBG_1",
																											"FIPS186_3_RSA_Prerequisite_DRBG_2"
																											});
			if (algo != null) algorithms.Add(algo);

			//186-2 SigGen
			algo = ParseAlgorithmFromModes("RSASigGen186-2", new List<string> { "REVALONLY_FIPS186_2SigGen_mod4096", "REVALONLY_FIPS186_2SigGenPKCS15_mod4096", "REVALONLY_FIPS186_2SigGenPKCSPSS_mod4096" }, new List<string> {"REVALONLY_FIPS186_2SigGen_mod4096",
																																																								"REVALONLY_FIPS186_2SigGen_mod4096SHA224",
																																																								"REVALONLY_FIPS186_2SigGen_mod4096SHA256",
																																																								"REVALONLY_FIPS186_2SigGen_mod4096SHA384",
																																																								"REVALONLY_FIPS186_2SigGen_mod4096SHA512",
																																																								"REVALONLY_FIPS186_2SigGenPKCS15_mod4096",
																																																								"REVALONLY_FIPS186_2SigGenPKCS15_mod4096SHA224",
																																																								"REVALONLY_FIPS186_2SigGenPKCS15_mod4096SHA256",
																																																								"REVALONLY_FIPS186_2SigGenPKCS15_mod4096SHA384",
																																																								"REVALONLY_FIPS186_2SigGenPKCS15_mod4096SHA512",
																																																								"REVALONLY_FIPS186_2SigGenPKCSPSS_mod4096",
																																																								"REVALONLY_FIPS186_2SigGenPKCSPSS_mod4096SHA224",
																																																								"REVALONLY_FIPS186_2SigGenPKCSPSS_mod4096SHA256",
																																																								"REVALONLY_FIPS186_2SigGenPKCSPSS_mod4096SHA384",
																																																								"REVALONLY_FIPS186_2SigGenPKCSPSS_mod4096SHA512",
																																																								"REVALONLY_FIPS186_2SigGenPKCSPSS_mod4096SHA224SaltLen",
																																																								"REVALONLY_FIPS186_2SigGenPKCSPSS_mod4096SHA256SaltLen",
																																																								"REVALONLY_FIPS186_2SigGenPKCSPSS_mod4096SHA384SaltLen",
																																																								"REVALONLY_FIPS186_2SigGenPKCSPSS_mod4096SHA512SaltLen",
																																																								"FIPS186_3_RSA_Prerequisite_SHA_1",
																																																								"FIPS186_3_RSA_Prerequisite_SHA_2",
																																																								"FIPS186_3_RSA_Prerequisite_SHA_3"
																																																								});
			//This really should check the SHA checkboxes to see if it really gets added
			if (algo != null) algorithms.Add(algo);


			//KeyGen
			algo = ParseAlgorithmFromMode("RSAKeyGen", "FIPS186_3KeyGen", new List<string> {"FIPS186_3KeyGen_ProbRP",
																							"FIPS186_3KeyGen_ProvRP",
																							"FIPS186_3KeyGen_ProvPC",
																							"FIPS186_3KeyGen_BothPC",
																							"FIPS186_3KeyGen_ProbPC",
																							"RSA2_Fixed_e",
																							"RSA2_Fixed_e_Value",
																							"RSA2_Random_e",
																							"RSA2_ProbRP_Mod2048",
																							"RSA2_ProbRP_Mod3072",
																							"RSA2_ProbRP_TableC2",
																							"RSA2_ProbRP_TableC3",
																							"RSA2_ProbRP_Fixed_e",
																							"RSA2_ProbRP_Random_e",
																							"RSA2_ProvRP_Mod2048SHA1",
																							"RSA2_ProvRP_Mod2048SHA224",
																							"RSA2_ProvRP_Mod2048SHA256",
																							"RSA2_ProvRP_Mod2048SHA384",
																							"RSA2_ProvRP_Mod2048SHA512",
																							"RSA2_ProvRP_Mod2048SHA512224",
																							"RSA2_ProvRP_Mod2048SHA512256",
																							"RSA2_ProvRP_Mod3072SHA1",
																							"RSA2_ProvRP_Mod3072SHA224",
																							"RSA2_ProvRP_Mod3072SHA256",
																							"RSA2_ProvRP_Mod3072SHA384",
																							"RSA2_ProvRP_Mod3072SHA512",
																							"RSA2_ProvRP_Mod3072SHA512224",
																							"RSA2_ProvRP_Mod3072SHA512256",
																							"RSA2_ProvPC_Mod2048SHA1",
																							"RSA2_ProvPC_Mod2048SHA224",
																							"RSA2_ProvPC_Mod2048SHA256",
																							"RSA2_ProvPC_Mod2048SHA384",
																							"RSA2_ProvPC_Mod2048SHA512",
																							"RSA2_ProvPC_Mod2048SHA512224",
																							"RSA2_ProvPC_Mod2048SHA512256",
																							"RSA2_ProvPC_Mod3072SHA1",
																							"RSA2_ProvPC_Mod3072SHA224",
																							"RSA2_ProvPC_Mod3072SHA256",
																							"RSA2_ProvPC_Mod3072SHA384",
																							"RSA2_ProvPC_Mod3072SHA512",
																							"RSA2_ProvPC_Mod3072SHA512224",
																							"RSA2_ProvPC_Mod3072SHA512256",
																							"RSA2_BothPC_Mod2048SHA1",
																							"RSA2_BothPC_Mod2048SHA224",
																							"RSA2_BothPC_Mod2048SHA256",
																							"RSA2_BothPC_Mod2048SHA384",
																							"RSA2_BothPC_Mod2048SHA512",
																							"RSA2_BothPC_Mod2048SHA512224",
																							"RSA2_BothPC_Mod2048SHA512256",
																							"RSA2_BothPC_Mod3072SHA1",
																							"RSA2_BothPC_Mod3072SHA224",
																							"RSA2_BothPC_Mod3072SHA256",
																							"RSA2_BothPC_Mod3072SHA384",
																							"RSA2_BothPC_Mod3072SHA512",
																							"RSA2_BothPC_Mod3072SHA512224",
																							"RSA2_BothPC_Mod3072SHA512256",
																							"RSA2_BothPC_TableC2",
																							"RSA2_BothPC_TableC3",
																							"RSA2_ProbPC_Mod2048",
																							"RSA2_ProbPC_Mod3072",
																							"RSA2_ProbPC_TableC2",
																							"RSA2_ProbPC_TableC3",
																							"FIPS186_3_RSA_Prerequisite_SHA_1",
																							"FIPS186_3_RSA_Prerequisite_SHA_2",
																							"FIPS186_3_RSA_Prerequisite_SHA_3",
																							"FIPS186_3_RSA_Prerequisite_DRBG_1",
																							"FIPS186_3_RSA_Prerequisite_DRBG_2"
																							});

			//We've seen submisions that select 186-4 KeyGen but then don't have any prime generation modes selected. CAVS winds up not testing these. So need to catch that case and bail out.
			if (algo != null && (algo.Options.GetValue("FIPS186_3KeyGen_ProbRP") == "True"
								|| algo.Options.GetValue("FIPS186_3KeyGen_ProvRP") == "True"
								|| algo.Options.GetValue("FIPS186_3KeyGen_ProvPC") == "True"
								|| algo.Options.GetValue("FIPS186_3KeyGen_BothPC") == "True"
								|| algo.Options.GetValue("FIPS186_3KeyGen_ProbPC") == "True"))
			{
				algorithms.Add(algo);
			}


			//SigGen - need to do this if any of the 3 SigGen modes are done
			algo = ParseAlgorithmFromModes("RSASigGen", new List<string> { "FIPS186_3SigGen", "FIPS186_3SigGenPKCS15", "FIPS186_3SigGenPKCSPSS" }, new List<string> {   "FIPS186_3SigGen",
																																										"FIPS186_3SigGen_mod2048",
																																										"FIPS186_3SigGen_mod2048SHA1",
																																										"FIPS186_3SigGen_mod2048SHA224",
																																										"FIPS186_3SigGen_mod2048SHA256",
																																										"FIPS186_3SigGen_mod2048SHA384",
																																										"FIPS186_3SigGen_mod2048SHA512",
																																										"FIPS186_3SigGen_mod2048SHA512224",
																																										"FIPS186_3SigGen_mod2048SHA512256",
																																										"FIPS186_3SigGen_mod3072",
																																										"FIPS186_3SigGen_mod3072SHA1",
																																										"FIPS186_3SigGen_mod3072SHA224",
																																										"FIPS186_3SigGen_mod3072SHA256",
																																										"FIPS186_3SigGen_mod3072SHA384",
																																										"FIPS186_3SigGen_mod3072SHA512",
																																										"FIPS186_3SigGen_mod3072SHA512224",
																																										"FIPS186_3SigGen_mod3072SHA512256",
																																										"FIPS186_3SigGenPKCS15",
																																										"FIPS186_3SigGenPKCS15_mod2048",
																																										"FIPS186_3SigGenPKCS15_mod2048SHA1",
																																										"FIPS186_3SigGenPKCS15_mod2048SHA224",
																																										"FIPS186_3SigGenPKCS15_mod2048SHA256",
																																										"FIPS186_3SigGenPKCS15_mod2048SHA384",
																																										"FIPS186_3SigGenPKCS15_mod2048SHA512",
																																										"FIPS186_3SigGenPKCS15_mod2048SHA512224",
																																										"FIPS186_3SigGenPKCS15_mod2048SHA512256",
																																										"FIPS186_3SigGenPKCS15_mod3072",
																																										"FIPS186_3SigGenPKCS15_mod3072SHA1",
																																										"FIPS186_3SigGenPKCS15_mod3072SHA224",
																																										"FIPS186_3SigGenPKCS15_mod3072SHA256",
																																										"FIPS186_3SigGenPKCS15_mod3072SHA384",
																																										"FIPS186_3SigGenPKCS15_mod3072SHA512",
																																										"FIPS186_3SigGenPKCS15_mod3072SHA512224",
																																										"FIPS186_3SigGenPKCS15_mod3072SHA512256",
																																										"FIPS186_3SigGenPKCSPSS",
																																										"FIPS186_3SigGenPKCSPSS_mod2048",
																																										"FIPS186_3SigGenPKCSPSS_mod2048SHA1",
																																										"FIPS186_3SigGenPKCSPSS_mod2048SHA224",
																																										"FIPS186_3SigGenPKCSPSS_mod2048SHA256",
																																										"FIPS186_3SigGenPKCSPSS_mod2048SHA384",
																																										"FIPS186_3SigGenPKCSPSS_mod2048SHA512",
																																										"FIPS186_3SigGenPKCSPSS_mod2048SHA512224",
																																										"FIPS186_3SigGenPKCSPSS_mod2048SHA512256",
																																										"FIPS186_3SigGenPKCSPSS_mod2048SHA1SaltLen",
																																										"FIPS186_3SigGenPKCSPSS_mod2048SHA224SaltLen",
																																										"FIPS186_3SigGenPKCSPSS_mod2048SHA256SaltLen",
																																										"FIPS186_3SigGenPKCSPSS_mod2048SHA384SaltLen",
																																										"FIPS186_3SigGenPKCSPSS_mod2048SHA512SaltLen",
																																										"FIPS186_3SigGenPKCSPSS_mod2048SHA512224SaltLen",
																																										"FIPS186_3SigGenPKCSPSS_mod2048SHA512256SaltLen",
																																										"FIPS186_3SigGenPKCSPSS_mod3072",
																																										"FIPS186_3SigGenPKCSPSS_mod3072SHA1",
																																										"FIPS186_3SigGenPKCSPSS_mod3072SHA224",
																																										"FIPS186_3SigGenPKCSPSS_mod3072SHA256",
																																										"FIPS186_3SigGenPKCSPSS_mod3072SHA384",
																																										"FIPS186_3SigGenPKCSPSS_mod3072SHA512",
																																										"FIPS186_3SigGenPKCSPSS_mod3072SHA512224",
																																										"FIPS186_3SigGenPKCSPSS_mod3072SHA512256",
																																										"FIPS186_3SigGenPKCSPSS_mod3072SHA1SaltLen",
																																										"FIPS186_3SigGenPKCSPSS_mod3072SHA224SaltLen",
																																										"FIPS186_3SigGenPKCSPSS_mod3072SHA256SaltLen",
																																										"FIPS186_3SigGenPKCSPSS_mod3072SHA384SaltLen",
																																										"FIPS186_3SigGenPKCSPSS_mod3072SHA512SaltLen",
																																										"FIPS186_3SigGenPKCSPSS_mod3072SHA512224SaltLen",
																																										"FIPS186_3SigGenPKCSPSS_mod3072SHA512256SaltLen",
																																										"FIPS186_3_RSA_Prerequisite_SHA_1",
																																										"FIPS186_3_RSA_Prerequisite_SHA_2",
																																										"FIPS186_3_RSA_Prerequisite_SHA_3"
			});

			if (algo != null) algorithms.Add(algo);

			//SigVer - do if any of the 3 SigVer modes are done
			algo = ParseAlgorithmFromModes("RSASigVer", new List<string> { "FIPS186_3SigVer", "FIPS186_3SigVerPKCS15", "FIPS186_3SigVerPKCSPSS" }, new List<string> {   "FIPS186_3SigVer",
																																										"FIPS186_3SigVer_Random_e_Value",
																																										"FIPS186_3SigVer_Fixed_e_Value",
																																										"FIPS186_3SigVer_Fixed_e_Value_Min",
																																										"FIPS186_3SigVer_Fixed_e_Value_Max",
																																										"FIPS186_3SigVer_mod1024",
																																										"FIPS186_3SigVer_mod1024SHA1",
																																										"FIPS186_3SigVer_mod1024SHA224",
																																										"FIPS186_3SigVer_mod1024SHA256",
																																										"FIPS186_3SigVer_mod1024SHA384",
																																										"FIPS186_3SigVer_mod1024SHA512",
																																										"FIPS186_3SigVer_mod1024SHA512224",
																																										"FIPS186_3SigVer_mod1024SHA512256",
																																										"FIPS186_3SigVer_mod2048",
																																										"FIPS186_3SigVer_mod2048SHA1",
																																										"FIPS186_3SigVer_mod2048SHA224",
																																										"FIPS186_3SigVer_mod2048SHA256",
																																										"FIPS186_3SigVer_mod2048SHA384",
																																										"FIPS186_3SigVer_mod2048SHA512",
																																										"FIPS186_3SigVer_mod2048SHA512224",
																																										"FIPS186_3SigVer_mod2048SHA512256",
																																										"FIPS186_3SigVer_mod3072",
																																										"FIPS186_3SigVer_mod3072SHA1",
																																										"FIPS186_3SigVer_mod3072SHA224",
																																										"FIPS186_3SigVer_mod3072SHA256",
																																										"FIPS186_3SigVer_mod3072SHA384",
																																										"FIPS186_3SigVer_mod3072SHA512",
																																										"FIPS186_3SigVer_mod3072SHA512224",
																																										"FIPS186_3SigVer_mod3072SHA512256",
																																										"FIPS186_3SigVerPKCS15",
																																										"FIPS186_3SigVerPKCS15_Random_e_Value",
																																										"FIPS186_3SigVerPKCS15_Fixed_e_Value",
																																										"FIPS186_3SigVerPKCS15_Fixed_e_Value_Min",
																																										"FIPS186_3SigVerPKCS15_Fixed_e_Value_Max",
																																										"FIPS186_3SigVerPKCS15_mod1024",
																																										"FIPS186_3SigVerPKCS15_mod1024SHA1",
																																										"FIPS186_3SigVerPKCS15_mod1024SHA224",
																																										"FIPS186_3SigVerPKCS15_mod1024SHA256",
																																										"FIPS186_3SigVerPKCS15_mod1024SHA384",
																																										"FIPS186_3SigVerPKCS15_mod1024SHA512",
																																										"FIPS186_3SigVerPKCS15_mod1024SHA512224",
																																										"FIPS186_3SigVerPKCS15_mod1024SHA512256",
																																										"FIPS186_3SigVerPKCS15_mod2048",
																																										"FIPS186_3SigVerPKCS15_mod2048SHA1",
																																										"FIPS186_3SigVerPKCS15_mod2048SHA224",
																																										"FIPS186_3SigVerPKCS15_mod2048SHA256",
																																										"FIPS186_3SigVerPKCS15_mod2048SHA384",
																																										"FIPS186_3SigVerPKCS15_mod2048SHA512",
																																										"FIPS186_3SigVerPKCS15_mod2048SHA512224",
																																										"FIPS186_3SigVerPKCS15_mod2048SHA512256",
																																										"FIPS186_3SigVerPKCS15_mod3072",
																																										"FIPS186_3SigVerPKCS15_mod3072SHA1",
																																										"FIPS186_3SigVerPKCS15_mod3072SHA224",
																																										"FIPS186_3SigVerPKCS15_mod3072SHA256",
																																										"FIPS186_3SigVerPKCS15_mod3072SHA384",
																																										"FIPS186_3SigVerPKCS15_mod3072SHA512",
																																										"FIPS186_3SigVerPKCS15_mod3072SHA512224",
																																										"FIPS186_3SigVerPKCS15_mod3072SHA512256",
																																										"FIPS186_3SigVerPKCSPSS",
																																										"FIPS186_3SigVerPKCSPSS_Random_e_Value",
																																										"FIPS186_3SigVerPKCSPSS_Fixed_e_Value",
																																										"FIPS186_3SigVerPKCSPSS_Fixed_e_Value_Min",
																																										"FIPS186_3SigVerPKCSPSS_Fixed_e_Value_Max",
																																										"FIPS186_3SigVerPKCSPSS_mod1024",
																																										"FIPS186_3SigVerPKCSPSS_mod1024SHA1",
																																										"FIPS186_3SigVerPKCSPSS_mod1024SHA224",
																																										"FIPS186_3SigVerPKCSPSS_mod1024SHA256",
																																										"FIPS186_3SigVerPKCSPSS_mod1024SHA384",
																																										"FIPS186_3SigVerPKCSPSS_mod1024SHA512",
																																										"FIPS186_3SigVerPKCSPSS_mod1024SHA512224",
																																										"FIPS186_3SigVerPKCSPSS_mod1024SHA512256",
																																										"FIPS186_3SigVerPKCSPSS_mod1024SHA1SaltLen",
																																										"FIPS186_3SigVerPKCSPSS_mod1024SHA224SaltLen",
																																										"FIPS186_3SigVerPKCSPSS_mod1024SHA256SaltLen",
																																										"FIPS186_3SigVerPKCSPSS_mod1024SHA384SaltLen",
																																										"FIPS186_3SigVerPKCSPSS_mod1024SHA512SaltLen",
																																										"FIPS186_3SigVerPKCSPSS_mod1024SHA512224SaltLen",
																																										"FIPS186_3SigVerPKCSPSS_mod1024SHA512256SaltLen",
																																										"FIPS186_3SigVerPKCSPSS_mod2048",
																																										"FIPS186_3SigVerPKCSPSS_mod2048SHA1",
																																										"FIPS186_3SigVerPKCSPSS_mod2048SHA224",
																																										"FIPS186_3SigVerPKCSPSS_mod2048SHA256",
																																										"FIPS186_3SigVerPKCSPSS_mod2048SHA384",
																																										"FIPS186_3SigVerPKCSPSS_mod2048SHA512",
																																										"FIPS186_3SigVerPKCSPSS_mod2048SHA512224",
																																										"FIPS186_3SigVerPKCSPSS_mod2048SHA512256",
																																										"FIPS186_3SigVerPKCSPSS_mod2048SHA1SaltLen",
																																										"FIPS186_3SigVerPKCSPSS_mod2048SHA224SaltLen",
																																										"FIPS186_3SigVerPKCSPSS_mod2048SHA256SaltLen",
																																										"FIPS186_3SigVerPKCSPSS_mod2048SHA384SaltLen",
																																										"FIPS186_3SigVerPKCSPSS_mod2048SHA512SaltLen",
																																										"FIPS186_3SigVerPKCSPSS_mod2048SHA512224SaltLen",
																																										"FIPS186_3SigVerPKCSPSS_mod2048SHA512256SaltLen",
																																										"FIPS186_3SigVerPKCSPSS_mod3072",
																																										"FIPS186_3SigVerPKCSPSS_mod3072SHA1",
																																										"FIPS186_3SigVerPKCSPSS_mod3072SHA224",
																																										"FIPS186_3SigVerPKCSPSS_mod3072SHA256",
																																										"FIPS186_3SigVerPKCSPSS_mod3072SHA384",
																																										"FIPS186_3SigVerPKCSPSS_mod3072SHA512",
																																										"FIPS186_3SigVerPKCSPSS_mod3072SHA512224",
																																										"FIPS186_3SigVerPKCSPSS_mod3072SHA512256",
																																										"FIPS186_3SigVerPKCSPSS_mod3072SHA1SaltLen",
																																										"FIPS186_3SigVerPKCSPSS_mod3072SHA224SaltLen",
																																										"FIPS186_3SigVerPKCSPSS_mod3072SHA256SaltLen",
																																										"FIPS186_3SigVerPKCSPSS_mod3072SHA384SaltLen",
																																										"FIPS186_3SigVerPKCSPSS_mod3072SHA512SaltLen",
																																										"FIPS186_3SigVerPKCSPSS_mod3072SHA512224SaltLen",
																																										"FIPS186_3SigVerPKCSPSS_mod3072SHA512256SaltLen",
																																										"FIPS186_3_RSA_Prerequisite_SHA_1",
																																										"FIPS186_3_RSA_Prerequisite_SHA_2",
																																										"FIPS186_3_RSA_Prerequisite_SHA_3"
																																									});
			if (algo != null) algorithms.Add(algo);


			//LegacySigVer
			algo = ParseAlgorithmFromMode("RSALegacySigVer", "RSALegacy", new List<string> {"Legacy_FIPS186_2SigVer",
																							"Legacy_FIPS186_2SigVer_SHA1",
																							"Legacy_FIPS186_2SigVer_SHA224",
																							"Legacy_FIPS186_2SigVer_SHA256",
																							"Legacy_FIPS186_2SigVer_SHA384",
																							"Legacy_FIPS186_2SigVer_SHA512",
																							"Legacy_FIPS186_2SigVer_mod1024",
																							"Legacy_FIPS186_2SigVer_mod1536",
																							"Legacy_FIPS186_2SigVer_mod2048",
																							"Legacy_FIPS186_2SigVer_mod3072",
																							"Legacy_FIPS186_2SigVer_mod4096",
																							"Legacy_PKCS#1_15SigVer",
																							"Legacy_PKCS#1_15SigVer_SHA1",
																							"Legacy_PKCS#1_15SigVer_SHA224",
																							"Legacy_PKCS#1_15SigVer_SHA256",
																							"Legacy_PKCS#1_15SigVer_SHA384",
																							"Legacy_PKCS#1_15SigVer_SHA512",
																							"Legacy_PKCS#1_15SigVer_mod1024",
																							"Legacy_PKCS#1_15SigVer_mod1536",
																							"Legacy_PKCS#1_15SigVer_mod2048",
																							"Legacy_PKCS#1_15SigVer_mod3072",
																							"Legacy_PKCS#1_15SigVer_mod4096",
																							"Legacy_PKCS#1_PSSSigVer",
																							"Legacy_PKCS#1_PSSSigVer_SHA1",
																							"Legacy_PKCS#1_PSSSigVer_SHA224",
																							"Legacy_PKCS#1_PSSSigVer_SHA256",
																							"Legacy_PKCS#1_PSSSigVer_SHA384",
																							"Legacy_PKCS#1_PSSSigVer_SHA512",
																							"Legacy_PKCS#1_PSSSigVer_mod1024",
																							"Legacy_PKCS#1_PSSSigVer_mod1536",
																							"Legacy_PKCS#1_PSSSigVer_mod2048",
																							"Legacy_PKCS#1_PSSSigVer_mod3072",
																							"Legacy_PKCS#1_PSSSigVer_mod4096"
																							});

			//Having seen cases where they select the top level checkboxes but not the children, make sure that enough is selected before actually including this algorithm
			if (algo != null
				&& ((algo.Options.GetValue("Legacy_FIPS186_2SigVer") == "True"
						&& (algo.Options.GetValue("Legacy_FIPS186_2SigVer_SHA1") == "True"
							|| algo.Options.GetValue("Legacy_FIPS186_2SigVer_SHA224") == "True"
							|| algo.Options.GetValue("Legacy_FIPS186_2SigVer_SHA256") == "True"
							|| algo.Options.GetValue("Legacy_FIPS186_2SigVer_SHA384") == "True"
							|| algo.Options.GetValue("Legacy_FIPS186_2SigVer_SHA512") == "True")
						&& (algo.Options.GetValue("Legacy_FIPS186_2SigVer_mod1024") == "True"
							|| algo.Options.GetValue("Legacy_FIPS186_2SigVer_mod1536") == "True"
							|| algo.Options.GetValue("Legacy_FIPS186_2SigVer_mod2048") == "True"
							|| algo.Options.GetValue("Legacy_FIPS186_2SigVer_mod3072") == "True"
							|| algo.Options.GetValue("Legacy_FIPS186_2SigVer_mod4096") == "True"))
					|| (algo.Options.GetValue("Legacy_PKCS#1_15SigVer") == "True"
						&& (algo.Options.GetValue("Legacy_PKCS#1_15SigVer_SHA1") == "True"
							|| algo.Options.GetValue("Legacy_PKCS#1_15SigVer_SHA224") == "True"
							|| algo.Options.GetValue("Legacy_PKCS#1_15SigVer_SHA256") == "True"
							|| algo.Options.GetValue("Legacy_PKCS#1_15SigVer_SHA384") == "True"
							|| algo.Options.GetValue("Legacy_PKCS#1_15SigVer_SHA512") == "True")
						&& (algo.Options.GetValue("Legacy_PKCS#1_15SigVer_mod1024") == "True"
							|| algo.Options.GetValue("Legacy_PKCS#1_15SigVer_mod1536") == "True"
							|| algo.Options.GetValue("Legacy_PKCS#1_15SigVer_mod2048") == "True"
							|| algo.Options.GetValue("Legacy_PKCS#1_15SigVer_mod3072") == "True"
							|| algo.Options.GetValue("Legacy_PKCS#1_15SigVer_mod4096") == "True"))
					|| (algo.Options.GetValue("Legacy_PKCS#1_PSSSigVer") == "True"
						&& (algo.Options.GetValue("Legacy_PKCS#1_PSSSigVer_SHA1") == "True"
							|| algo.Options.GetValue("Legacy_PKCS#1_PSSSigVer_SHA224") == "True"
							|| algo.Options.GetValue("Legacy_PKCS#1_PSSSigVer_SHA256") == "True"
							|| algo.Options.GetValue("Legacy_PKCS#1_PSSSigVer_SHA384") == "True"
							|| algo.Options.GetValue("Legacy_PKCS#1_PSSSigVer_SHA512") == "True")
						&& (algo.Options.GetValue("Legacy_PKCS#1_PSSSigVer_mod1024") == "True"
							|| algo.Options.GetValue("Legacy_PKCS#1_PSSSigVer_mod1536") == "True"
							|| algo.Options.GetValue("Legacy_PKCS#1_PSSSigVer_mod2048") == "True"
							|| algo.Options.GetValue("Legacy_PKCS#1_PSSSigVer_mod3072") == "True"
							|| algo.Options.GetValue("Legacy_PKCS#1_PSSSigVer_mod4096") == "True"))
						)) { algorithms.Add(algo); }



			return algorithms;
		}
	}
}