using System.Collections.Generic;

namespace LCAVPCore.AlgorithmChunkParsers
{
	public class RSAComponent_RSADPChunkParser : AlgorithmChunkParserBase, IAlgorithmChunkParser
	{
		public RSAComponent_RSADPChunkParser(InfFileSection chunk) : base(chunk)
		{
		}

		public List<InfAlgorithm> Parse()
		{
			return new List<InfAlgorithm> { new InfAlgorithm("RSADP", Chunk.KeyValuePairs) };
		}
	}
}