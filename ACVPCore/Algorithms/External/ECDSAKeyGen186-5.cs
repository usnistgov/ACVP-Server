using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ACVPCore.Algorithms.External
{
	public class ECDSAKeyGen186_5 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("curve")]
		public List<string> Curves { get; set; }

		[JsonPropertyName("secretGenerationMode")]
		public List<string> SecretGenerationMode { get; set; }

		public ECDSAKeyGen186_5()
		{
			Name = "ECDSA";
			Mode = "keyGen";
			Revision = "FIPS186-5";
		}
	}
}
