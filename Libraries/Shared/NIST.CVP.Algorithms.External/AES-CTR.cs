using System.Collections.Generic;
using System.Text.Json.Serialization;
using NIST.CVP.Algorithms.DataTypes;

namespace NIST.CVP.Algorithms.External
{
	public class AES_CTR : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("direction")]
		public List<string> Direction { get; set; }

		[JsonPropertyName("keyLen")]
		public List<int> KeyLength { get; set; }

		[JsonPropertyName("payloadLen")]
		public Domain PayloadLength { get; set; }

		[JsonPropertyName("overflowCounter")]
		public bool? OverflowCounter { get; set; }

		[JsonPropertyName("incrementalCounter")]
		public bool? IncrementalCounter { get; set; }

		public AES_CTR()
		{
			Name = "ACVP-AES-CTR";
			Revision = "1.0";
		}
	}
}
