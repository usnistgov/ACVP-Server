using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ACVPCore.Algorithms.External
{
	public class TDES_CBC : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("direction")]
		public List<string> Direction { get; set; }

		[JsonPropertyName("keyingOption")]
		public List<int> KeyingOption { get; set; }

		[JsonPropertyName("ptLen")]
		public List<int> PTLength { get; set; }

		public TDES_CBC()
		{
			Name = "ACVP-TDES-CBC";
			Revision = "1.0";
		}
	}
}
