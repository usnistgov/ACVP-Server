using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ACVPCore.Algorithms.External
{
	public class TDES_CFB64 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("direction")]
		public List<string> Direction { get; set; }

		[JsonPropertyName("keyingOption")]
		public List<int> KeyingOption { get; set; }

		public TDES_CFB64()
		{
			Name = "ACVP-TDES-CFB64";
			Revision = "1.0";
		}
	}
}
