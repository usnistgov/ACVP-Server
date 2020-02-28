using System.Collections.Generic;

namespace LCAVPCore.AlgorithmChunkParsers
{
	public interface IAlgorithmChunkParser
	{
		List<InfAlgorithm> Parse();
	}
}