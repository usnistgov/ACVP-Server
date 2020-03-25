using System.Collections.Generic;
using System.Linq;

namespace LCAVPCore.AlgorithmChunkParsers
{
	public class HMACChunkParser : AlgorithmChunkParserBase, IAlgorithmChunkParser
	{
		public HMACChunkParser(InfFileSection chunk) : base(chunk)
		{
		}

		public List<InfAlgorithm> Parse()
		{
			List<InfAlgorithm> algorithms = new List<InfAlgorithm>();

			//A few are simple, with just a mode type of line indicating their use
			algorithms.Add(ParseAlgorithmFromMode("HMAC-SHA-1", "HMAC_SHA1", new List<string> { "HMAC_SHA_Prerequisite", "HMAC_SHA3_Prerequisite", "HMAC_SHA1_K<B_1", "HMAC_SHA1_K<B_2", "HMAC_SHA1_K>B_1", "HMAC_SHA1_K>B_2", "HMAC_SHA1_K_EQ_B", "HMAC_SHA1_MAC10", "HMAC_SHA1_MAC12", "HMAC_SHA1_MAC16", "HMAC_SHA1_MAC20" }));
			algorithms.Add(ParseAlgorithmFromMode("HMAC-SHA2-512/224", "HMAC_SHA512_224", new List<string> { "HMAC_SHA_Prerequisite", "HMAC_SHA3_Prerequisite", "HMAC_SHA512_224_K<B_1", "HMAC_SHA512_224_K<B_2", "HMAC_SHA512_224_K>B_1", "HMAC_SHA512_224_K>B_2", "HMAC_SHA512_224_K_EQ_B", "HMAC_SHA512_224_MAC14", "HMAC_SHA512_224_MAC16", "HMAC_SHA512_224_MAC20", "HMAC_SHA512_224_MAC24", "HMAC_SHA512_224_MAC28" }));
			algorithms.Add(ParseAlgorithmFromMode("HMAC-SHA2-512/256", "HMAC_SHA512_256", new List<string> { "HMAC_SHA_Prerequisite", "HMAC_SHA3_Prerequisite", "HMAC_SHA512_256_K<B_1", "HMAC_SHA512_256_K<B_2", "HMAC_SHA512_256_K>B_1", "HMAC_SHA512_256_K>B_2", "HMAC_SHA512_256_K_EQ_B", "HMAC_SHA512_256_MAC16", "HMAC_SHA512_256_MAC24", "HMAC_SHA512_256_MAC32" }));

			//All the others require looking at a SHA2Tested and SHA3Tested flag to determine if should get it. Going to trust that the parent option is checked (HMAC_SHA224 checked if either HMAC_SHA224_SHA2Tested or HMAC_SHA224_SHA3Tested checked)
			algorithms.Add(ParseAlgorithmFromMode("HMAC-SHA2-224", "HMAC_SHA224_SHA2Tested", new List<string> { "HMAC_SHA_Prerequisite", "HMAC_SHA3_Prerequisite", "HMAC_SHA224_SHA2Tested", "HMAC_SHA224_K<B_1", "HMAC_SHA224_K<B_2", "HMAC_SHA224_K>B_1", "HMAC_SHA224_K>B_2", "HMAC_SHA224_K_EQ_B", "HMAC_SHA224_MAC14", "HMAC_SHA224_MAC16", "HMAC_SHA224_MAC20", "HMAC_SHA224_MAC24", "HMAC_SHA224_MAC28" }));
			algorithms.Add(ParseAlgorithmFromMode("HMAC-SHA3-224", "HMAC_SHA224_SHA3Tested", new List<string> { "HMAC_SHA_Prerequisite", "HMAC_SHA3_Prerequisite", "HMAC_SHA224_SHA3Tested", "HMAC_SHA224_K<B_1", "HMAC_SHA224_K<B_2", "HMAC_SHA224_K>B_1", "HMAC_SHA224_K>B_2", "HMAC_SHA224_K_EQ_B", "HMAC_SHA224_MAC14", "HMAC_SHA224_MAC16", "HMAC_SHA224_MAC20", "HMAC_SHA224_MAC24", "HMAC_SHA224_MAC28" }));
			algorithms.Add(ParseAlgorithmFromMode("HMAC-SHA2-256", "HMAC_SHA256_SHA2Tested", new List<string> { "HMAC_SHA_Prerequisite", "HMAC_SHA3_Prerequisite", "HMAC_SHA256_SHA2Tested", "HMAC_SHA256_K<B_1", "HMAC_SHA256_K<B_2", "HMAC_SHA256_K>B_1", "HMAC_SHA256_K>B_2", "HMAC_SHA256_K_EQ_B", "HMAC_SHA256_MAC16", "HMAC_SHA256_MAC24", "HMAC_SHA256_MAC32" }));
			algorithms.Add(ParseAlgorithmFromMode("HMAC-SHA3-256", "HMAC_SHA256_SHA3Tested", new List<string> { "HMAC_SHA_Prerequisite", "HMAC_SHA3_Prerequisite", "HMAC_SHA256_SHA3Tested", "HMAC_SHA256_K<B_1", "HMAC_SHA256_K<B_2", "HMAC_SHA256_K>B_1", "HMAC_SHA256_K>B_2", "HMAC_SHA256_K_EQ_B", "HMAC_SHA256_MAC16", "HMAC_SHA256_MAC24", "HMAC_SHA256_MAC32" }));
			algorithms.Add(ParseAlgorithmFromMode("HMAC-SHA2-384", "HMAC_SHA384_SHA2Tested", new List<string> { "HMAC_SHA_Prerequisite", "HMAC_SHA3_Prerequisite", "HMAC_SHA384_SHA2Tested", "HMAC_SHA384_K<B_1", "HMAC_SHA384_K<B_2", "HMAC_SHA384_K>B_1", "HMAC_SHA384_K>B_2", "HMAC_SHA384_K_EQ_B", "HMAC_SHA384_MAC24", "HMAC_SHA384_MAC32", "HMAC_SHA384_MAC40", "HMAC_SHA384_MAC48" }));
			algorithms.Add(ParseAlgorithmFromMode("HMAC-SHA3-384", "HMAC_SHA384_SHA3Tested", new List<string> { "HMAC_SHA_Prerequisite", "HMAC_SHA3_Prerequisite", "HMAC_SHA384_SHA3Tested", "HMAC_SHA384_K<B_1", "HMAC_SHA384_K<B_2", "HMAC_SHA384_K>B_1", "HMAC_SHA384_K>B_2", "HMAC_SHA384_K_EQ_B", "HMAC_SHA384_MAC24", "HMAC_SHA384_MAC32", "HMAC_SHA384_MAC40", "HMAC_SHA384_MAC48" }));
			algorithms.Add(ParseAlgorithmFromMode("HMAC-SHA2-512", "HMAC_SHA512_SHA2Tested", new List<string> { "HMAC_SHA_Prerequisite", "HMAC_SHA3_Prerequisite", "HMAC_SHA512_SHA2Tested", "HMAC_SHA512_K<B_1", "HMAC_SHA512_K<B_2", "HMAC_SHA512_K>B_1", "HMAC_SHA512_K>B_2", "HMAC_SHA512_K_EQ_B", "HMAC_SHA512_MAC32", "HMAC_SHA512_MAC40", "HMAC_SHA512_MAC48", "HMAC_SHA512_MAC56", "HMAC_SHA512_MAC64" }));
			algorithms.Add(ParseAlgorithmFromMode("HMAC-SHA3-512", "HMAC_SHA512_SHA3Tested", new List<string> { "HMAC_SHA_Prerequisite", "HMAC_SHA3_Prerequisite", "HMAC_SHA512_SHA3Tested", "HMAC_SHA512_K<B_1", "HMAC_SHA512_K<B_2", "HMAC_SHA512_K>B_1", "HMAC_SHA512_K>B_2", "HMAC_SHA512_K_EQ_B", "HMAC_SHA512_MAC32", "HMAC_SHA512_MAC40", "HMAC_SHA512_MAC48", "HMAC_SHA512_MAC56", "HMAC_SHA512_MAC64" }));

			//Since we might have added nulls to collection, for modes that were not "selected", filter them out before returning
			return algorithms.Where(x => x != null).ToList();
		}
	}
}