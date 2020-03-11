using System.Collections.Generic;

namespace ACVPCore.Algorithms.Persisted
{
	public class EDDSAKeyGen : PersistedAlgorithmBase
	{
		[AlgorithmProperty("curve")]
		public List<string> Curves { get; set; }

		[AlgorithmProperty("secretGenerationMode")]
		public List<string> SecretGenerationMode { get; set; }

		public EDDSAKeyGen()
		{
			Name = "EDDSA";
			Mode = "keyGen";
			Revision = "1.0";
		}

		public EDDSAKeyGen(ACVPCore.Algorithms.External.EDDSAKeyGen external) : this()
		{
			Curves = external.Curves;
			SecretGenerationMode = external.SecretGenerationMode;
		}
	}
}
