using System.Collections.Generic;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmChunkParsers
{
	public class ECDSA2ChunkParser : AlgorithmChunkParserBase, IAlgorithmChunkParser
	{
		public ECDSA2ChunkParser(InfFileSection chunk) : base(chunk)
		{
		}

		public List<InfAlgorithm> Parse()
		{
			//ECDSA is a single algorithm in the new model, so just need to convert the format
			//return new List<InfAlgorithm> { new InfAlgorithm("ECDSA", Chunk.KeyValuePairs) };

			//Read through the chunk, build up algorithms
			List<InfAlgorithm> algorithms = new List<InfAlgorithm>();
			InfAlgorithm algo;

			//KeyPair
			algo = ParseAlgorithmFromMode("ECDSAKeyPair", "KeyPair", new List<string> {	"KeyPair_P-192",
																						"KeyPair_P-224",
																						"KeyPair_P-256",
																						"KeyPair_P-384",
																						"KeyPair_P-521",
																						"KeyPair_K-163",
																						"KeyPair_K-233",
																						"KeyPair_K-283",
																						"KeyPair_K-409",
																						"KeyPair_K-571",
																						"KeyPair_B-163",
																						"KeyPair_B-233",
																						"KeyPair_B-283",
																						"KeyPair_B-409",
																						"KeyPair_B-571",
																						"KeyPair_ExtraRandomBits",
																						"KeyPair_TestingCandidates",
																						"REVALONLY_KeyPair_FIPS_186_2",
																						"ECDSA2_Prerequisite_DRBG"
																						});

			//Make sure at least one of the options is true before adding
			if (algo != null && algo.Options.ContainsValue("True"))
			{
				algorithms.Add(algo);
			}

			//PKV
			algo = ParseAlgorithmFromMode("ECDSAPKV", "PKV", new List<string> { "PKV_P-192",
																				"PKV_P-224",
																				"PKV_P-256",
																				"PKV_P-384",
																				"PKV_P-521",
																				"PKV_K-163",
																				"PKV_K-233",
																				"PKV_K-283",
																				"PKV_K-409",
																				"PKV_K-571",
																				"PKV_B-163",
																				"PKV_B-233",
																				"PKV_B-283",
																				"PKV_B-409",
																				"PKV_B-571",
																				});

			//Make sure at least one of the options is true before adding
			if (algo != null && algo.Options.ContainsValue("True"))
			{
				algorithms.Add(algo);
			}

			//SigGen
			algo = ParseAlgorithmFromMode("ECDSASigGen", "SigGen", new List<string> {	"SigGen_P-192_SHA-1",
																						"SigGen_P-192_SHA-224",
																						"SigGen_P-192_SHA-256",
																						"SigGen_P-192_SHA-384",
																						"SigGen_P-192_SHA-512",
																						"SigGen_P-192_SHA-512224",
																						"SigGen_P-192_SHA-512256",
																						"SigGen_P-224_SHA-1",
																						"SigGen_P-224_SHA-224",
																						"SigGen_P-224_SHA-256",
																						"SigGen_P-224_SHA-384",
																						"SigGen_P-224_SHA-512",
																						"SigGen_P-224_SHA-512224",
																						"SigGen_P-224_SHA-512256",
																						"SigGen_P-256_SHA-1",
																						"SigGen_P-256_SHA-224",
																						"SigGen_P-256_SHA-256",
																						"SigGen_P-256_SHA-384",
																						"SigGen_P-256_SHA-512",
																						"SigGen_P-256_SHA-512224",
																						"SigGen_P-256_SHA-512256",
																						"SigGen_P-384_SHA-1",
																						"SigGen_P-384_SHA-224",
																						"SigGen_P-384_SHA-256",
																						"SigGen_P-384_SHA-384",
																						"SigGen_P-384_SHA-512",
																						"SigGen_P-384_SHA-512224",
																						"SigGen_P-384_SHA-512256",
																						"SigGen_P-521_SHA-1",
																						"SigGen_P-521_SHA-224",
																						"SigGen_P-521_SHA-256",
																						"SigGen_P-521_SHA-384",
																						"SigGen_P-521_SHA-512",
																						"SigGen_P-521_SHA-512224",
																						"SigGen_P-521_SHA-512256",
																						"SigGen_K-163_SHA-1",
																						"SigGen_K-163_SHA-224",
																						"SigGen_K-163_SHA-256",
																						"SigGen_K-163_SHA-384",
																						"SigGen_K-163_SHA-512",
																						"SigGen_K-163_SHA-512224",
																						"SigGen_K-163_SHA-512256",
																						"SigGen_K-233_SHA-1",
																						"SigGen_K-233_SHA-224",
																						"SigGen_K-233_SHA-256",
																						"SigGen_K-233_SHA-384",
																						"SigGen_K-233_SHA-512",
																						"SigGen_K-233_SHA-512224",
																						"SigGen_K-233_SHA-512256",
																						"SigGen_K-283_SHA-1",
																						"SigGen_K-283_SHA-224",
																						"SigGen_K-283_SHA-256",
																						"SigGen_K-283_SHA-384",
																						"SigGen_K-283_SHA-512",
																						"SigGen_K-283_SHA-512224",
																						"SigGen_K-283_SHA-512256",
																						"SigGen_K-409_SHA-1",
																						"SigGen_K-409_SHA-224",
																						"SigGen_K-409_SHA-256",
																						"SigGen_K-409_SHA-384",
																						"SigGen_K-409_SHA-512",
																						"SigGen_K-409_SHA-512224",
																						"SigGen_K-409_SHA-512256",
																						"SigGen_K-571_SHA-1",
																						"SigGen_K-571_SHA-224",
																						"SigGen_K-571_SHA-256",
																						"SigGen_K-571_SHA-384",
																						"SigGen_K-571_SHA-512",
																						"SigGen_K-571_SHA-512224",
																						"SigGen_K-571_SHA-512256",
																						"SigGen_B-163_SHA-1",
																						"SigGen_B-163_SHA-224",
																						"SigGen_B-163_SHA-256",
																						"SigGen_B-163_SHA-384",
																						"SigGen_B-163_SHA-512",
																						"SigGen_B-163_SHA-512224",
																						"SigGen_B-163_SHA-512256",
																						"SigGen_B-233_SHA-1",
																						"SigGen_B-233_SHA-224",
																						"SigGen_B-233_SHA-256",
																						"SigGen_B-233_SHA-384",
																						"SigGen_B-233_SHA-512",
																						"SigGen_B-233_SHA-512224",
																						"SigGen_B-233_SHA-512256",
																						"SigGen_B-283_SHA-1",
																						"SigGen_B-283_SHA-224",
																						"SigGen_B-283_SHA-256",
																						"SigGen_B-283_SHA-384",
																						"SigGen_B-283_SHA-512",
																						"SigGen_B-283_SHA-512224",
																						"SigGen_B-283_SHA-512256",
																						"SigGen_B-409_SHA-1",
																						"SigGen_B-409_SHA-224",
																						"SigGen_B-409_SHA-256",
																						"SigGen_B-409_SHA-384",
																						"SigGen_B-409_SHA-512",
																						"SigGen_B-409_SHA-512224",
																						"SigGen_B-409_SHA-512256",
																						"SigGen_B-571_SHA-1",
																						"SigGen_B-571_SHA-224",
																						"SigGen_B-571_SHA-256",
																						"SigGen_B-571_SHA-384",
																						"SigGen_B-571_SHA-512",
																						"SigGen_B-571_SHA-512224",
																						"SigGen_B-571_SHA-512256",
																						"ECDSA2_Prerequisite_SHA_1",
																						"ECDSA2_Prerequisite_SHA_2",
																						"ECDSA2_Prerequisite_SHA_3",
																						"ECDSA2_Prerequisite_DRBG",
																						"ECDSA2_Prerequisite_DRBG2"
																						});

			//Make sure at least one of the options is true before adding
			if (algo != null && algo.Options.ContainsValue("True"))
			{
				algorithms.Add(algo);
			}


			//SigVer
			algo = ParseAlgorithmFromMode("ECDSASigVer", "SigVer", new List<string> {   "SigVer_P-192_SHA-1",
																						"SigVer_P-192_SHA-224",
																						"SigVer_P-192_SHA-256",
																						"SigVer_P-192_SHA-384",
																						"SigVer_P-192_SHA-512",
																						"SigVer_P-192_SHA-512224",
																						"SigVer_P-192_SHA-512256",
																						"SigVer_P-224_SHA-1",
																						"SigVer_P-224_SHA-224",
																						"SigVer_P-224_SHA-256",
																						"SigVer_P-224_SHA-384",
																						"SigVer_P-224_SHA-512",
																						"SigVer_P-224_SHA-512224",
																						"SigVer_P-224_SHA-512256",
																						"SigVer_P-256_SHA-1",
																						"SigVer_P-256_SHA-224",
																						"SigVer_P-256_SHA-256",
																						"SigVer_P-256_SHA-384",
																						"SigVer_P-256_SHA-512",
																						"SigVer_P-256_SHA-512224",
																						"SigVer_P-256_SHA-512256",
																						"SigVer_P-384_SHA-1",
																						"SigVer_P-384_SHA-224",
																						"SigVer_P-384_SHA-256",
																						"SigVer_P-384_SHA-384",
																						"SigVer_P-384_SHA-512",
																						"SigVer_P-384_SHA-512224",
																						"SigVer_P-384_SHA-512256",
																						"SigVer_P-521_SHA-1",
																						"SigVer_P-521_SHA-224",
																						"SigVer_P-521_SHA-256",
																						"SigVer_P-521_SHA-384",
																						"SigVer_P-521_SHA-512",
																						"SigVer_P-521_SHA-512224",
																						"SigVer_P-521_SHA-512256",
																						"SigVer_K-163_SHA-1",
																						"SigVer_K-163_SHA-224",
																						"SigVer_K-163_SHA-256",
																						"SigVer_K-163_SHA-384",
																						"SigVer_K-163_SHA-512",
																						"SigVer_K-163_SHA-512224",
																						"SigVer_K-163_SHA-512256",
																						"SigVer_K-233_SHA-1",
																						"SigVer_K-233_SHA-224",
																						"SigVer_K-233_SHA-256",
																						"SigVer_K-233_SHA-384",
																						"SigVer_K-233_SHA-512",
																						"SigVer_K-233_SHA-512224",
																						"SigVer_K-233_SHA-512256",
																						"SigVer_K-283_SHA-1",
																						"SigVer_K-283_SHA-224",
																						"SigVer_K-283_SHA-256",
																						"SigVer_K-283_SHA-384",
																						"SigVer_K-283_SHA-512",
																						"SigVer_K-283_SHA-512224",
																						"SigVer_K-283_SHA-512256",
																						"SigVer_K-409_SHA-1",
																						"SigVer_K-409_SHA-224",
																						"SigVer_K-409_SHA-256",
																						"SigVer_K-409_SHA-384",
																						"SigVer_K-409_SHA-512",
																						"SigVer_K-409_SHA-512224",
																						"SigVer_K-409_SHA-512256",
																						"SigVer_K-571_SHA-1",
																						"SigVer_K-571_SHA-224",
																						"SigVer_K-571_SHA-256",
																						"SigVer_K-571_SHA-384",
																						"SigVer_K-571_SHA-512",
																						"SigVer_K-571_SHA-512224",
																						"SigVer_K-571_SHA-512256",
																						"SigVer_B-163_SHA-1",
																						"SigVer_B-163_SHA-224",
																						"SigVer_B-163_SHA-256",
																						"SigVer_B-163_SHA-384",
																						"SigVer_B-163_SHA-512",
																						"SigVer_B-163_SHA-512224",
																						"SigVer_B-163_SHA-512256",
																						"SigVer_B-233_SHA-1",
																						"SigVer_B-233_SHA-224",
																						"SigVer_B-233_SHA-256",
																						"SigVer_B-233_SHA-384",
																						"SigVer_B-233_SHA-512",
																						"SigVer_B-233_SHA-512224",
																						"SigVer_B-233_SHA-512256",
																						"SigVer_B-283_SHA-1",
																						"SigVer_B-283_SHA-224",
																						"SigVer_B-283_SHA-256",
																						"SigVer_B-283_SHA-384",
																						"SigVer_B-283_SHA-512",
																						"SigVer_B-283_SHA-512224",
																						"SigVer_B-283_SHA-512256",
																						"SigVer_B-409_SHA-1",
																						"SigVer_B-409_SHA-224",
																						"SigVer_B-409_SHA-256",
																						"SigVer_B-409_SHA-384",
																						"SigVer_B-409_SHA-512",
																						"SigVer_B-409_SHA-512224",
																						"SigVer_B-409_SHA-512256",
																						"SigVer_B-571_SHA-1",
																						"SigVer_B-571_SHA-224",
																						"SigVer_B-571_SHA-256",
																						"SigVer_B-571_SHA-384",
																						"SigVer_B-571_SHA-512",
																						"SigVer_B-571_SHA-512224",
																						"SigVer_B-571_SHA-512256",
																						"ECDSA2_Prerequisite_SHA_1",
																						"ECDSA2_Prerequisite_SHA_2",
																						"ECDSA2_Prerequisite_SHA_3"
																						});

			//Make sure at least one of the options is true before adding
			if (algo != null && algo.Options.ContainsValue("True"))
			{
				algorithms.Add(algo);
			}


			return algorithms;
		}
	}
}