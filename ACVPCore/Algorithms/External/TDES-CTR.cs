using System.Collections.Generic;
using System.Text.Json.Serialization;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.External
{
	public class TDES_CTR : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("direction")]
		public List<string> Direction { get; set; }

		[JsonPropertyName("keyingOption")]
		public List<int> KeyingOption { get; set; }

		[JsonPropertyName("payloadLen")]
		public Domain PayloadLength { get; set; }

		[JsonPropertyName("overflowCounter")]
		public bool? OverflowCounter { get; set; }

		[JsonPropertyName("incrementalCounter")]
		public bool? IncrementalCounter { get; set; }

		public TDES_CTR()
		{
			Name = "ACVP-TDES-CTR";
			Revision = "1.0";
		}
	}
}
