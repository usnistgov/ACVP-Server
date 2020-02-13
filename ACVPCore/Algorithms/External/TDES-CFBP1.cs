using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ACVPCore.Algorithms.External
{
	public class TDES_CFBP1 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("direction")]
		public List<string> Direction { get; set; }

		[JsonPropertyName("keyingOption")]
		public List<int> KeyingOption { get; set; }

		public TDES_CFBP1()
		{
			Name = "ACVP-TDES-CFBP1";
			Revision = "1.0";
		}
	}
}
