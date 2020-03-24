using System.Collections.Generic;
using External = NIST.CVP.Algorithms.External;

namespace NIST.CVP.Algorithms.Persisted
{
	public class ECDSAKeyGen186_4 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("curve")]
		public List<string> Curves { get; set; }

		[AlgorithmProperty("secretGenerationMode")]
		public List<string> SecretGenerationMode { get; set; }

		public ECDSAKeyGen186_4()
		{
			Name = "ECDSA";
			Mode = "keyGen";
			Revision = "1.0";
		}

		public ECDSAKeyGen186_4(External.ECDSAKeyGen186_4 external) : this()
		{
			Curves = external.Curves;
			SecretGenerationMode = external.SecretGenerationMode;
		}
	}
}
