using System.Collections.Generic;
using External = NIST.CVP.Algorithms.External;

namespace NIST.CVP.Algorithms.Persisted
{
	public class RSADecryptionPrimitive : PersistedAlgorithmBase
	{
		[AlgorithmProperty("modLen")]
		public int ModLength { get; set; } = 2048;

		public RSADecryptionPrimitive()
		{
			Name = "RSA";
			Mode = "decryptionPrimitive";
			Revision = "1.0";
		}

		public RSADecryptionPrimitive(External.RSADecryptionPrimitive external) : this()
		{
			ModLength = external.ModLength;
		}
	}
}
