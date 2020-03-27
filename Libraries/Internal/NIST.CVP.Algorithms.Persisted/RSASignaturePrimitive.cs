using System.Collections.Generic;
using External = NIST.CVP.Algorithms.External;

namespace NIST.CVP.Algorithms.Persisted
{
	public class RSASignaturePrimitive : PersistedAlgorithmBase
	{
		[AlgorithmProperty("keyFormat")]
		public string KeyFormat { get; set; }

		[AlgorithmProperty("paddingAlgorithm")]
		public List<string> PaddingAlgorithms { get; set; }

		public RSASignaturePrimitive()
		{
			Name = "RSA";
			Mode = "signaturePrimitive";
			Revision = "1.0";
		}

		public RSASignaturePrimitive(External.RSASignaturePrimitive external) : this()
		{
			KeyFormat = external.KeyFormat;
		}
	}
}
