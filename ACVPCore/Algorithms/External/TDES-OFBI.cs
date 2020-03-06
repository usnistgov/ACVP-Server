using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ACVPCore.Algorithms.External
{
	public class TDES_OFBI : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("direction")]
		public List<string> Direction { get; set; }

		[JsonPropertyName("keyingOption")]
		public List<int> KeyingOption { get; set; }

		public TDES_OFBI()
		{
			Name = "ACVP-TDES-OFBI";
			Revision = "1.0";
		}
	}
}
