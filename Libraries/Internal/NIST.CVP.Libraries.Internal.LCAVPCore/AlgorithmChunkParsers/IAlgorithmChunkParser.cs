using System.Collections.Generic;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmChunkParsers
{
	public interface IAlgorithmChunkParser
	{
		List<InfAlgorithm> Parse();
	}
}