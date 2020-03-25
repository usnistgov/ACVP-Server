using System.Collections.Generic;
using System.Linq;

namespace LCAVPCore.AlgorithmChunkParsers
{
	public class KeyWrapChunkParser : AlgorithmChunkParserBase, IAlgorithmChunkParser
	{
		public KeyWrapChunkParser(InfFileSection chunk) : base(chunk)
		{
		}

		public List<InfAlgorithm> Parse()
		{
			//Read through the chunk, build up algorithms
			List<InfAlgorithm> algorithms = new List<InfAlgorithm>();

			algorithms.Add(ParseAlgorithmFromModes("AES-KW", new List<string> { "KW_AE", "KW_AD" }, new List<string> { "AES_Prerequisite", "KW_AE", "KW_AD", "KW_FWD_CIPHER", "KW_INV_CIPHER", "KW_AES_128", "KW_AES_192", "KW_AES_256", "KW_PTLEN0", "KW_PTLEN1", "KW_PTLEN2", "KW_PTLEN3", "KW_PTLEN4" }));
			algorithms.Add(ParseAlgorithmFromModes("AES-KWP", new List<string> { "KWP_AE", "KWP_AD" }, new List<string> { "AES_Prerequisite", "KWP_AE", "KWP_AD", "KWP_FWD_CIPHER", "KWP_INV_CIPHER", "KWP_AES_128", "KWP_AES_192", "KWP_AES_256", "KWP_PTLEN0", "KWP_PTLEN1", "KWP_PTLEN2", "KWP_PTLEN3", "KWP_PTLEN4" }));
			algorithms.Add(ParseAlgorithmFromModes("TDES-KW", new List<string> { "TKW_AE", "TKW_AD" }, new List<string> { "AES_Prerequisite", "TKW_AE", "TKW_AD", "TKW_FWD_CIPHER", "TKW_INV_CIPHER", "TKW_PTLEN0", "TKW_PTLEN1", "TKW_PTLEN2", "TKW_PTLEN3", "TKW_PTLEN4" }));


			//Since we might have added nulls to collection, for modes that were not "selected", filter them out before returning
			return algorithms.Where(x => x != null).ToList();
		}
	}
}