using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NIST.CVP.Algorithms.External
{
	public class AES_CFB8 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("direction")]
		public List<string> Direction { get; set; }

		[JsonPropertyName("keyLen")]
		public List<int> KeyLength { get; set; }

		public AES_CFB8()
		{
			Name = "ACVP-AES-CFB8";
			Revision = "1.0";
		}
	}
}
