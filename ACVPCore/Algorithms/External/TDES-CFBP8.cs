using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ACVPCore.Algorithms.External
{
	public class TDES_CFBP8 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("direction")]
		public List<string> Direction { get; set; }

		[JsonPropertyName("keyingOption")]
		public List<int> KeyingOption { get; set; }

		public TDES_CFBP8()
		{
			Name = "ACVP-TDES-CFBP8";
			Revision = "1.0";
		}
	}
}
