using System.Collections.Generic;
using System.Text.Json.Serialization;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;

namespace NIST.CVP.Libraries.Shared.Algorithms.External
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
