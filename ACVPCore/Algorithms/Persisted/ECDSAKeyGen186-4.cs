using System.Collections.Generic;

namespace ACVPCore.Algorithms.Persisted
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

		public ECDSAKeyGen186_4(ACVPCore.Algorithms.External.ECDSAKeyGen186_4 external) : this()
		{
			Curves = external.Curves;
			SecretGenerationMode = external.SecretGenerationMode;
		}
	}
}
