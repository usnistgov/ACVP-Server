using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ACVPCore.Algorithms.External
{
	public class AES_CBC : : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("direction")]
		public List<string> Direction { get; set; }

		[JsonPropertyName("keyLen")]
		public List<long> KeyLength { get; set; }

		public AES_CBC()
		{
			Name = "ACVP-AES-CBC";
			Revision = "1.0";
		}
	}
}
