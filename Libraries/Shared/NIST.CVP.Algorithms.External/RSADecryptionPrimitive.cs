using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NIST.CVP.Algorithms.External
{
	public class RSADecryptionPrimitive : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("modLen")]
		public int ModLength { get; set; }

		public RSADecryptionPrimitive()
		{
			Name = "RSA";
			Mode = "decryptionPrimitive";
			Revision = "1.0";
		}
	}
}
