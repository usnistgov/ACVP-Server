using System.Collections.Generic;
using System.Text.Json.Serialization;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.External
{
	public class AES_XPN : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("direction")]
		public List<string> Direction { get; set; }

		[JsonPropertyName("keyLen")]
		public List<int> KeyLength { get; set; }

		[JsonPropertyName("payloadLen")]
		public Domain PayloadLength { get; set; }

		[JsonPropertyName("aadLen")]
		public Domain AADLength { get; set; }

		[JsonPropertyName("tagLen")]
		public List<int> TagLength { get; set; }

		[JsonPropertyName("ivGen")]
		public string IVGen { get; set; }

		[JsonPropertyName("ivGenMode")]
		public string IVGenMode { get; set; }

		[JsonPropertyName("saltGen")]
		public string SaltGeneration { get; set; }


		public AES_XPN()
		{
			Name = "ACVP-AES-XPN";
			Revision = "1.0";
		}
	}
}
