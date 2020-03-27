using System.Collections.Generic;
using System.Linq;

namespace LCAVPCore.AlgorithmChunkParsers
{
	public class TDESChunkParser : AlgorithmChunkParserBase, IAlgorithmChunkParser
	{
		public TDESChunkParser(InfFileSection chunk) : base(chunk)
		{
		}

		public List<InfAlgorithm> Parse()
		{
			//Read through the chunk, build up algorithms
			List<InfAlgorithm> algorithms = new List<InfAlgorithm>();

			algorithms.Add(ParseAlgorithmFromMode("TDES-ECB", "ECB_Mode", new List<string> { "ECB_State", "ECB_KEY_CHOICE1", "ECB_KEY_CHOICE2" }));
			algorithms.Add(ParseAlgorithmFromMode("TDES-CBC", "CBC_Mode", new List<string> { "CBC_State", "CBC_KEY_CHOICE1", "CBC_KEY_CHOICE2" }));
			algorithms.Add(ParseAlgorithmFromMode("TDES-CBC-I", "CBCI_Mode", new List<string> { "CBCI_State", "CBCI_KEY_CHOICE1", "CBCI_KEY_CHOICE2" }));
			algorithms.Add(ParseAlgorithmFromMode("TDES-OFB", "OFB_Mode", new List<string> { "OFB_State", "OFB_KEY_CHOICE1", "OFB_KEY_CHOICE2" }));
			algorithms.Add(ParseAlgorithmFromMode("TDES-OFB-I", "OFBI_Mode", new List<string> { "OFBI_State", "OFBI_KEY_CHOICE1", "OFBI_KEY_CHOICE2" }));
			algorithms.Add(ParseAlgorithmFromMode("TDES-CTR", "CTR_Mode", new List<string> { "CTR_State", "CTR_Src", "CTR_Descr" }));

			//CFB_Mode plus the CFP states being <> False controls 3 algorithm variants
			//algorithms.Add(ParseAlgorithmFromMode("TDES-CFB1", "CFB_Mode", new List<string> { "CFB1_State", "CFB_KEY_CHOICE1", "CFB_KEY_CHOICE2" }));
			//algorithms.Add(ParseAlgorithmFromMode("TDES-CFB8", "CFB_Mode", new List<string> { "CFB8_State", "CFB_KEY_CHOICE1", "CFB_KEY_CHOICE2" }));
			//algorithms.Add(ParseAlgorithmFromMode("TDES-CFB64", "CFB_Mode", new List<string> { "CFB64_State", "CFB_KEY_CHOICE1", "CFB_KEY_CHOICE2" }));

			InfAlgorithm algo = ParseAlgorithmFromMode("TDES-CFB1", "CFB_Mode", new List<string> { "CFB1_State", "CFB_KEY_CHOICE1", "CFB_KEY_CHOICE2" });
			if (algo != null && algo.Options.GetValue("CFB1_State") != "False") algorithms.Add(algo);

			algo = ParseAlgorithmFromMode("TDES-CFB8", "CFB_Mode", new List<string> { "CFB8_State", "CFB_KEY_CHOICE1", "CFB_KEY_CHOICE2" });
			if(algo != null && algo.Options.GetValue("CFB8_State") != "False") algorithms.Add(algo);

			algo = ParseAlgorithmFromMode("TDES-CFB64", "CFB_Mode", new List<string> { "CFB64_State", "CFB_KEY_CHOICE1", "CFB_KEY_CHOICE2" });
			if (algo != null && algo.Options.GetValue("CFB64_State") != "False") algorithms.Add(algo);


			//CFBP_Mode plus CFBP states control 3 algorithm variants
			//algorithms.Add(ParseAlgorithmFromMode("TDES-CFB-P1", "CFBP_Mode", new List<string> { "CFBP1_State", "CFBP_KEY_CHOICE1", "CFBP_KEY_CHOICE2" }));
			//algorithms.Add(ParseAlgorithmFromMode("TDES-CFB-P8", "CFBP_Mode", new List<string> { "CFBP8_State", "CFBP_KEY_CHOICE1", "CFBP_KEY_CHOICE2" }));
			//algorithms.Add(ParseAlgorithmFromMode("TDES-CFB-P64", "CFBP_Mode", new List<string> { "CFBP64_State", "CFBP_KEY_CHOICE1", "CFBP_KEY_CHOICE2" }));
			algo = ParseAlgorithmFromMode("TDES-CFBP1", "CFBP_Mode", new List<string> { "CFBP1_State", "CFBP_KEY_CHOICE1", "CFBP_KEY_CHOICE2" });
			if (algo != null && algo.Options.GetValue("CFBP1_State") != "False") algorithms.Add(algo);

			algo = ParseAlgorithmFromMode("TDES-CFBP8", "CFBP_Mode", new List<string> { "CFBP8_State", "CFBP_KEY_CHOICE1", "CFBP_KEY_CHOICE2" });
			if (algo != null && algo.Options.GetValue("CFBP8_State") != "False") algorithms.Add(algo);

			algo = ParseAlgorithmFromMode("TDES-CFBP64", "CFBP_Mode", new List<string> { "CFBP64_State", "CFBP_KEY_CHOICE1", "CFBP_KEY_CHOICE2" });
			if (algo != null && algo.Options.GetValue("CFBP64_State") != "False") algorithms.Add(algo);


			//Since we might have added nulls to collection, for modes that were not "selected", filter them out before returning
			return algorithms.Where(x => x != null).ToList();
		}
	}
}