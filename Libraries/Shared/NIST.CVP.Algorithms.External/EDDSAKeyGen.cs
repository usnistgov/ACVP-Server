using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NIST.CVP.Algorithms.External
{
	public class EDDSAKeyGen : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("curve")]
		public List<string> Curves { get; set; }

		[JsonPropertyName("secretGenerationMode")]
		public List<string> SecretGenerationMode { get; set; }

		public EDDSAKeyGen()
		{
			Name = "EDDSA";
			Mode = "keyGen";
			Revision = "1.0";
		}
	}
}
