using System.Collections.Generic;

namespace LCAVPCore.AlgorithmChunkParsers
{
	public class CCMChunkParser : AlgorithmChunkParserBase, IAlgorithmChunkParser
	{
		public CCMChunkParser(InfFileSection chunk) : base(chunk)
		{
		}

		public List<InfAlgorithm> Parse()
		{
			//CCM is a single algorithm, AES-CCM in the new model, so just need to convert the format
			return new List<InfAlgorithm> { new InfAlgorithm("AES-CCM", Chunk.KeyValuePairs) };
		}
	}
}