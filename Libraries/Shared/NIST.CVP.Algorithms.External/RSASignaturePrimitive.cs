using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NIST.CVP.Algorithms.External
{
	public class RSASignaturePrimitive : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("keyFormat")]
		public string KeyFormat { get; set; }

		public RSASignaturePrimitive()
		{
			Name = "RSA";
			Mode = "signaturePrimitive";
			Revision = "1.0";
		}
	}
}
