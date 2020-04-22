using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NIST.CVP.Libraries.Shared.Algorithms.External
{
	public class ECDSAKeyGen186_4 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("curve")]
		public List<string> Curves { get; set; }

		[JsonPropertyName("secretGenerationMode")]
		public List<string> SecretGenerationMode { get; set; }

		public ECDSAKeyGen186_4()
		{
			Name = "ECDSA";
			Mode = "keyGen";
			Revision = "1.0";
		}
	}
}
