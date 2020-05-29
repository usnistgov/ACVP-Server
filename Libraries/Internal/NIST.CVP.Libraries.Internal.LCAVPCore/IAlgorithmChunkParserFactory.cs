using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmChunkParsers;

namespace NIST.CVP.Libraries.Internal.LCAVPCore
{
	public interface IAlgorithmChunkParserFactory
	{
		IAlgorithmChunkParser GetParser(InfFileSection chunk);
	}
}