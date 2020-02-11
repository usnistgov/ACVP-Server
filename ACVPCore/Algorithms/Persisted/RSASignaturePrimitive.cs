using System.Collections.Generic;

namespace ACVPCore.Algorithms.Persisted
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

		public RSASignaturePrimitive(ACVPCore.Algorithms.External.RSASignaturePrimitive external) : this()
		{
			KeyFormat = external.KeyFormat;
		}
	}
}
