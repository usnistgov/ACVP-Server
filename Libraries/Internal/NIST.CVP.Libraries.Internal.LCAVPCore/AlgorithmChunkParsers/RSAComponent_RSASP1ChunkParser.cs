using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmChunkParsers
{
	public class RSAComponent_RSASP1ChunkParser : AlgorithmChunkParserBase, IAlgorithmChunkParser
	{
		public RSAComponent_RSASP1ChunkParser(InfFileSection chunk) : base(chunk)
		{
		}

		public List<InfAlgorithm> Parse()
		{
			//Read through the chunk, build up algorithms
			List<InfAlgorithm> algorithms = new List<InfAlgorithm>();

			algorithms.Add(ParseAlgorithmFromMode("RSASP1", "RSASP1", new List<string>()));		//Odd case in that we don't actually need any options back - giving it an empty collection prevents blowing up

			//Since we might have added nulls to collection, for modes that were not "selected", filter them out before returning
			return algorithms.Where(x => x != null).ToList();
		}
	}
}