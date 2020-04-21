using System.Collections.Generic;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmChunkParsers
{
	public class XTSChunkParser : AlgorithmChunkParserBase, IAlgorithmChunkParser
	{
		public XTSChunkParser(InfFileSection chunk) : base(chunk)
		{
		}

		public List<InfAlgorithm> Parse()
		{
			//XTS is a single algorithm (AES-XTS) in the new model, so just need to convert the format
			return new List<InfAlgorithm> { new InfAlgorithm("AES-XTS", Chunk.KeyValuePairs) };
		}
	}
}