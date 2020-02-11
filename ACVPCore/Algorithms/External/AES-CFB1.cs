using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ACVPCore.Algorithms.External
{
	public class AES_CFB1 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("direction")]
		public List<string> Direction { get; set; }

		[JsonPropertyName("keyLen")]
		public List<int> KeyLength { get; set; }

		public AES_CFB1()
		{
			Name = "ACVP-AES-CFB1";
			Revision = "1.0";
		}
	}
}
