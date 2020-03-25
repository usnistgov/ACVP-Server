using System.Collections.Generic;
using System.Text.Json.Serialization;
using NIST.CVP.Algorithms.DataTypes;

namespace NIST.CVP.Algorithms.External
{
	public class AES_GCM : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("direction")]
		public List<string> Direction { get; set; }

		[JsonPropertyName("ivGen")]
		public string IVGen { get; set; }

		[JsonPropertyName("ivGenMode")]
		public string IVGenMode { get; set; }

		[JsonPropertyName("keyLen")]
		public List<int> KeyLength { get; set; }

		[JsonPropertyName("tagLen")]
		public List<int> TagLength { get; set; }

		[JsonPropertyName("ivLen")]
		public Domain IVLength { get; set; }

		[JsonPropertyName("payloadLen")]
		public Domain PayloadLength { get; set; }

		[JsonPropertyName("aadLen")]
		public Domain AADLength { get; set; }

		public AES_GCM()
		{
			Name = "ACVP-AES-GCM";
			Revision = "1.0";
		}
	}
}
