using System.Collections.Generic;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmChunkParsers
{
	public class ECDSA2SigGenComponentChunkParser : AlgorithmChunkParserBase, IAlgorithmChunkParser
	{
		public ECDSA2SigGenComponentChunkParser(InfFileSection chunk) : base(chunk)
		{
		}

		public List<InfAlgorithm> Parse()
		{
			return new List<InfAlgorithm> { new InfAlgorithm("ECDSA SigGen Component", Chunk.KeyValuePairs) };
		}
	}
}