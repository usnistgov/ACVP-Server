using System.Collections.Generic;

namespace LCAVPCore.AlgorithmChunkParsers
{
	public class DSA2ChunkParser : AlgorithmChunkParserBase, IAlgorithmChunkParser
	{
		public DSA2ChunkParser(InfFileSection chunk) : base(chunk)
		{
		}

		public List<InfAlgorithm> Parse()
		{
			//DSA is a single algorithm in the new model, so just need to convert the format
			//return new List<InfAlgorithm> { new InfAlgorithm("DSA", Chunk.KeyValuePairs) };

			//Read through the chunk, build up algorithms
			List<InfAlgorithm> algorithms = new List<InfAlgorithm>();
			InfAlgorithm algo;

			//KeyPair
			algo = ParseAlgorithmFromMode("DSAKeyPair", "KeyPair", new List<string> {   "KeyPair_L1024N160",
																						"KeyPair_L2048N224",
																						"KeyPair_L2048N256",
																						"KeyPair_L3072N256"
																					});

			//Make sure at least one of the options is true before adding
			if (algo != null && algo.Options.ContainsValue("True"))
			{
				algorithms.Add(algo);
			}

			//PQGGen
			algo = ParseAlgorithmFromMode("DSAPQGGen", "PQGGen", new List<string> { "PQGGen_L1024N160_SHA-1",
																					"PQGGen_L1024N160_SHA-224",
																					"PQGGen_L1024N160_SHA-256",
																					"PQGGen_L1024N160_SHA-384",
																					"PQGGen_L1024N160_SHA-512",
																					"PQGGen_L1024N160_SHA-512224",
																					"PQGGen_L1024N160_SHA-512256",
																					"PQGGen_L2048N224_SHA-1",
																					"PQGGen_L2048N224_SHA-224",
																					"PQGGen_L2048N224_SHA-256",
																					"PQGGen_L2048N224_SHA-384",
																					"PQGGen_L2048N224_SHA-512",
																					"PQGGen_L2048N224_SHA-512224",
																					"PQGGen_L2048N224_SHA-512256",
																					"PQGGen_L2048N256_SHA-1",
																					"PQGGen_L2048N256_SHA-224",
																					"PQGGen_L2048N256_SHA-256",
																					"PQGGen_L2048N256_SHA-384",
																					"PQGGen_L2048N256_SHA-512",
																					"PQGGen_L2048N256_SHA-512224",
																					"PQGGen_L2048N256_SHA-512256",
																					"PQGGen_L3072N256_SHA-1",
																					"PQGGen_L3072N256_SHA-224",
																					"PQGGen_L3072N256_SHA-256",
																					"PQGGen_L3072N256_SHA-384",
																					"PQGGen_L3072N256_SHA-512",
																					"PQGGen_L3072N256_SHA-512224",
																					"PQGGen_L3072N256_SHA-512256",
																					"PQGGen_ProbablePrimePQ",
																					"PQGGen_ProvablePrimePQ",
																					"PQGGen_UnverifiableG",
																					"PQGGen_CanonicalG",
																					"DSA2_Prerequisite_SHA_1",
																					"DSA2_Prerequisite_SHA_2"
																					});

			//Make sure at least one of the options is true before adding
			if (algo != null && algo.Options.ContainsValue("True"))
			{
				algorithms.Add(algo);
			}

			//PQGVer
			algo = ParseAlgorithmFromMode("DSAPQGVer", "PQGVer", new List<string> { "PQGVer_L1024N160_SHA-1",
																					"PQGVer_L1024N160_SHA-224",
																					"PQGVer_L1024N160_SHA-256",
																					"PQGVer_L1024N160_SHA-384",
																					"PQGVer_L1024N160_SHA-512",
																					"PQGVer_L1024N160_SHA-512224",
																					"PQGVer_L1024N160_SHA-512256",
																					"PQGVer_L2048N224_SHA-1",
																					"PQGVer_L2048N224_SHA-224",
																					"PQGVer_L2048N224_SHA-256",
																					"PQGVer_L2048N224_SHA-384",
																					"PQGVer_L2048N224_SHA-512",
																					"PQGVer_L2048N224_SHA-512224",
																					"PQGVer_L2048N224_SHA-512256",
																					"PQGVer_L2048N256_SHA-1",
																					"PQGVer_L2048N256_SHA-224",
																					"PQGVer_L2048N256_SHA-256",
																					"PQGVer_L2048N256_SHA-384",
																					"PQGVer_L2048N256_SHA-512",
																					"PQGVer_L2048N256_SHA-512224",
																					"PQGVer_L2048N256_SHA-512256",
																					"PQGVer_L3072N256_SHA-1",
																					"PQGVer_L3072N256_SHA-224",
																					"PQGVer_L3072N256_SHA-256",
																					"PQGVer_L3072N256_SHA-384",
																					"PQGVer_L3072N256_SHA-512",
																					"PQGVer_L3072N256_SHA-512224",
																					"PQGVer_L3072N256_SHA-512256",
																					"PQGVer_ProbablePrimePQ",
																					"PQGVer_ProvablePrimePQ",
																					"PQGVer_UnverifiableG",
																					"PQGVer_CanonicalG",
																					"PQGVer_FIPS186-2PQGVerTest",
																					"DSA2_Prerequisite_SHA_1",
																					"DSA2_Prerequisite_SHA_2"
																					});

			//Make sure at least one of the options is true before adding
			if (algo != null && algo.Options.ContainsValue("True"))
			{
				algorithms.Add(algo);
			}

			//SigGen
			algo = ParseAlgorithmFromMode("DSASigGen", "SigGen", new List<string> { "SigGen_L1024N160_SHA-1",
																					"SigGen_L1024N160_SHA-224",
																					"SigGen_L1024N160_SHA-256",
																					"SigGen_L1024N160_SHA-384",
																					"SigGen_L1024N160_SHA-512",
																					"SigGen_L1024N160_SHA-512224",
																					"SigGen_L1024N160_SHA-512256",
																					"SigGen_L2048N224_SHA-1",
																					"SigGen_L2048N224_SHA-224",
																					"SigGen_L2048N224_SHA-256",
																					"SigGen_L2048N224_SHA-384",
																					"SigGen_L2048N224_SHA-512",
																					"SigGen_L2048N224_SHA-512224",
																					"SigGen_L2048N224_SHA-512256",
																					"SigGen_L2048N256_SHA-1",
																					"SigGen_L2048N256_SHA-224",
																					"SigGen_L2048N256_SHA-256",
																					"SigGen_L2048N256_SHA-384",
																					"SigGen_L2048N256_SHA-512",
																					"SigGen_L2048N256_SHA-512224",
																					"SigGen_L2048N256_SHA-512256",
																					"SigGen_L3072N256_SHA-1",
																					"SigGen_L3072N256_SHA-224",
																					"SigGen_L3072N256_SHA-256",
																					"SigGen_L3072N256_SHA-384",
																					"SigGen_L3072N256_SHA-512",
																					"SigGen_L3072N256_SHA-512224",
																					"SigGen_L3072N256_SHA-512256",
																					"DSA2_Prerequisite_SHA_1",
																					"DSA2_Prerequisite_SHA_2",
																					"DSA2_Prerequisite_DRBG"
																					});

			//Make sure at least one of the options is true before adding
			if (algo != null && algo.Options.ContainsValue("True"))
			{
				algorithms.Add(algo);
			}

			//SigVer
			algo = ParseAlgorithmFromMode("DSASigVer", "SigVer", new List<string> { "SigVer_L1024N160_SHA-1",
																					"SigVer_L1024N160_SHA-224",
																					"SigVer_L1024N160_SHA-256",
																					"SigVer_L1024N160_SHA-384",
																					"SigVer_L1024N160_SHA-512",
																					"SigVer_L1024N160_SHA-512224",
																					"SigVer_L1024N160_SHA-512256",
																					"SigVer_L2048N224_SHA-1",
																					"SigVer_L2048N224_SHA-224",
																					"SigVer_L2048N224_SHA-256",
																					"SigVer_L2048N224_SHA-384",
																					"SigVer_L2048N224_SHA-512",
																					"SigVer_L2048N224_SHA-512224",
																					"SigVer_L2048N224_SHA-512256",
																					"SigVer_L2048N256_SHA-1",
																					"SigVer_L2048N256_SHA-224",
																					"SigVer_L2048N256_SHA-256",
																					"SigVer_L2048N256_SHA-384",
																					"SigVer_L2048N256_SHA-512",
																					"SigVer_L2048N256_SHA-512224",
																					"SigVer_L2048N256_SHA-512256",
																					"SigVer_L3072N256_SHA-1",
																					"SigVer_L3072N256_SHA-224",
																					"SigVer_L3072N256_SHA-256",
																					"SigVer_L3072N256_SHA-384",
																					"SigVer_L3072N256_SHA-512",
																					"SigVer_L3072N256_SHA-512224",
																					"SigVer_L3072N256_SHA-512256",
																					"DSA2_Prerequisite_SHA_1",
																					"DSA2_Prerequisite_SHA_2"
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