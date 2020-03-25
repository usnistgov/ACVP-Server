using LCAVPCore.AlgorithmChunkParsers;

namespace LCAVPCore
{
	public interface IAlgorithmChunkParserFactory
	{
		IAlgorithmChunkParser GetParser(InfFileSection chunk);
	}
}