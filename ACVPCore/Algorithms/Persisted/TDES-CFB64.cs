using System.Collections.Generic;

namespace ACVPCore.Algorithms.Persisted
{
	public class TDES_CFB64 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("direction")]
		public List<string> Direction { get; set; }

		[AlgorithmProperty("key")]
		public List<int> KeyingOption { get; set; }

		public TDES_CFB64()
		{
			Name = "ACVP-TDES-CFB64";
			Revision = "1.0";
		}

		public TDES_CFB64(ACVPCore.Algorithms.External.TDES_CFB64 external) : this()
		{
			Direction = external.Direction;
			KeyingOption = external.KeyingOption;
		}
	}
}
