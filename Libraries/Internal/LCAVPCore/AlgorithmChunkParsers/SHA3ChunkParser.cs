using System.Collections.Generic;
using System.Linq;

namespace LCAVPCore.AlgorithmChunkParsers
{
	public class SHA3ChunkParser : AlgorithmChunkParserBase, IAlgorithmChunkParser
	{
		public SHA3ChunkParser(InfFileSection chunk) : base(chunk)
		{
		}

		public List<InfAlgorithm> Parse()
		{
			//Read through the chunk, build up algorithms
			List<InfAlgorithm> algorithms = new List<InfAlgorithm>();

			algorithms.Add(ParseAlgorithmFromMode("SHA3-224", "SHA3_224", new List<string> { "SHA3_224_Byte", "SHA3_NoNull" }));
			algorithms.Add(ParseAlgorithmFromMode("SHA3-256", "SHA3_256", new List<string> { "SHA3_256_Byte", "SHA3_NoNull" }));
			algorithms.Add(ParseAlgorithmFromMode("SHA3-384", "SHA3_384", new List<string> { "SHA3_384_Byte", "SHA3_NoNull" }));
			algorithms.Add(ParseAlgorithmFromMode("SHA3-512", "SHA3_512", new List<string> { "SHA3_512_Byte", "SHA3_NoNull" }));
			algorithms.Add(ParseAlgorithmFromMode("SHAKE-128", "SHAKE128", new List<string> { "SHAKE128_Byte", "SHAKE128_OutputByteOnly", "SHAKE128_Output_MinLen", "SHAKE128_Output_MaxLen", "SHAKE128_OutputMax2TO16", "SHAKE_NoNull" }));
			algorithms.Add(ParseAlgorithmFromMode("SHAKE-256", "SHAKE256", new List<string> { "SHAKE256_Byte", "SHAKE256_OutputByteOnly", "SHAKE256_Output_MinLen", "SHAKE256_Output_MaxLen", "SHAKE256_OutputMax2TO16", "SHAKE_NoNull" }));

			//Since we might have added nulls to collection, for modes that were not "selected", filter them out before returning
			return algorithms.Where(x => x != null).ToList();
		}
	}
}