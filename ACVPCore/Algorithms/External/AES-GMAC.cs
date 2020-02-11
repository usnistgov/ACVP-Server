using System.Collections.Generic;
using System.Text.Json.Serialization;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.External
{
	public class AES_GMAC : AlgorithmBase, IExternalAlgorithm
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


		[JsonPropertyName("aadLen")]
		public Domain AADLength { get; set; }

		public AES_GMAC()
		{
			Name = "ACVP-AES-GMAC";
			Revision = "1.0";
		}
	}
}
