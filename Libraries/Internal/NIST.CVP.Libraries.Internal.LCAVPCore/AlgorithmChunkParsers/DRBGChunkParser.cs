using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmChunkParsers
{
	public class DRBGChunkParser : AlgorithmChunkParserBase, IAlgorithmChunkParser
	{
		public DRBGChunkParser(InfFileSection chunk) : base(chunk)
		{
		}

		public List<InfAlgorithm> Parse()
		{
			List<InfAlgorithm> algorithms = new List<InfAlgorithm>();

			//If tested, CTR_DRBG needs all the lines starting with "CTR".
			algorithms.Add(ParseAlgorithmFromMode("CTR_DRBG", "CTR_DRBG", Chunk.KeyValuePairs.Select(x => x.Key).Where(x => x.StartsWith("CTR")).ToList()));

			//If tested, Hash_Based DRBG needs all the lines starting with "Hash".
			algorithms.Add(ParseAlgorithmFromMode("Hash_Based DRBG", "Hash_DRBG", Chunk.KeyValuePairs.Select(x => x.Key).Where(x => x.StartsWith("Hash")).ToList()));

			//If tested, HMAC_Based DRBG needs all the lines starting with "HMAC"
			algorithms.Add(ParseAlgorithmFromMode("HMAC_Based DRBG", "HMAC_DRBG", Chunk.KeyValuePairs.Select(x => x.Key).Where(x => x.StartsWith("HMAC")).ToList()));

			//Since we might have added nulls to collection, for modes that were not "selected", filter them out before returning
			return algorithms.Where(x => x != null).ToList();
		}
	}
}