using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmChunkParsers
{
	public class SHAChunkParser : AlgorithmChunkParserBase, IAlgorithmChunkParser
	{
		public SHAChunkParser(InfFileSection chunk) : base(chunk)
		{
		}

		public List<InfAlgorithm> Parse()
		{
			//Read through the chunk, build up algorithms
			List<InfAlgorithm> algorithms = new List<InfAlgorithm>();

			//SHA_NoNull is only specified once across all SHA in CAVS, but in new model it is at the algorithm level
			algorithms.Add(ParseAlgorithmFromMode("SHA-1", "SHA1", new List<string> { "SHA1_Byte", "SHA_NoNull" }));
			algorithms.Add(ParseAlgorithmFromMode("SHA-224", "SHA224", new List<string> { "SHA224_Byte", "SHA_NoNull" }));
			algorithms.Add(ParseAlgorithmFromMode("SHA-256", "SHA256", new List<string> { "SHA256_Byte", "SHA_NoNull" }));
			algorithms.Add(ParseAlgorithmFromMode("SHA-384", "SHA384", new List<string> { "SHA384_Byte", "SHA_NoNull" }));
			algorithms.Add(ParseAlgorithmFromMode("SHA-512", "SHA512", new List<string> { "SHA512_Byte", "SHA_NoNull" }));
			algorithms.Add(ParseAlgorithmFromMode("SHA-512/224", "SHA512_224", new List<string> { "SHA512_224_Byte", "SHA_NoNull" }));
			algorithms.Add(ParseAlgorithmFromMode("SHA-512/256", "SHA512_256", new List<string> { "SHA512_256_Byte", "SHA_NoNull" }));

			//Since we might have added nulls to collection, for modes that were not "selected", filter them out before returning
			return algorithms.Where(x => x != null).ToList();
		}
	}
}