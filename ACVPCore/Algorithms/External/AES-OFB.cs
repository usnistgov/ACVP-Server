using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ACVPCore.Algorithms.External
{
	public class AES_OFB : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("direction")]
		public List<string> Direction { get; set; }

		[JsonPropertyName("keyLen")]
		public List<int> KeyLength { get; set; }

		public AES_OFB()
		{
			Name = "ACVP-AES-OFB";
			Revision = "1.0";
		}
	}
}
