using System.Collections.Generic;

namespace ACVPCore.Algorithms.Persisted
{
	public class ECDSAKeyGen186_5 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("curve")]
		public List<string> Curves { get; set; }

		[AlgorithmProperty("secretGenerationMode")]
		public List<string> SecretGenerationMode { get; set; }

		public ECDSAKeyGen186_5()
		{
			Name = "ECDSA";
			Mode = "keyGen";
			Revision = "FIPS186-5";
		}

		public ECDSAKeyGen186_5(ACVPCore.Algorithms.External.ECDSAKeyGen186_5 external) : this()
		{
			Curves = external.Curves;
			SecretGenerationMode = external.SecretGenerationMode;
		}
	}
}
