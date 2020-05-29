using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmChunkParsers
{
	public class AESChunkParser : AlgorithmChunkParserBase, IAlgorithmChunkParser
	{
		public AESChunkParser(InfFileSection chunk) : base(chunk)
		{
		}

		public List<InfAlgorithm> Parse()
		{
			//Read through the chunk, build up algorithms
			List<InfAlgorithm> algorithms = new List<InfAlgorithm>();

			algorithms.Add(ParseAlgorithmFromMode("AES-ECB", "ECB_Mode", new List<string> { "ECB128_State", "ECB192_State", "ECB256_State" }));
			algorithms.Add(ParseAlgorithmFromMode("AES-CBC", "CBC_Mode", new List<string> { "CBC128_State", "CBC192_State", "CBC256_State" }));
			algorithms.Add(ParseAlgorithmFromMode("AES-OFB", "OFB_Mode", new List<string> { "OFB128_State", "OFB192_State", "OFB256_State" }));
			algorithms.Add(ParseAlgorithmFromMode("AES-CFB1", "CFB1_Mode", new List<string> { "CFB1_128_State", "CFB1_192_State", "CFB1_256_State" }));
			algorithms.Add(ParseAlgorithmFromMode("AES-CFB8", "CFB8_Mode", new List<string> { "CFB8_128_State", "CFB8_192_State", "CFB8_256_State" }));
			algorithms.Add(ParseAlgorithmFromMode("AES-CFB128", "CFB128_Mode", new List<string> { "CFB128_128_State", "CFB128_192_State", "CFB128_256_State" }));
			algorithms.Add(ParseAlgorithmFromMode("AES-CTR", "CTR_Mode", new List<string> { "CTR128_State", "CTR192_State", "CTR256_State", "CTR_Src" }));

			//Since we might have added nulls to collection, for modes that were not "selected", filter them out before returning
			return algorithms.Where(x => x != null).ToList();
		}
	}
}