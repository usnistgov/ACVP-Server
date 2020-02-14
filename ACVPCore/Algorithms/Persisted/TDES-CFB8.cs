using System.Collections.Generic;

namespace ACVPCore.Algorithms.Persisted
{
	public class TDES_CFB8 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("direction")]
		public List<string> Direction { get; set; }

		[AlgorithmProperty("key")]
		public List<int> KeyingOption { get; set; }

		public TDES_CFB8()
		{
			Name = "ACVP-TDES-CFB8";
			Revision = "1.0";
		}

		public TDES_CFB8(ACVPCore.Algorithms.External.TDES_CFB8 external) : this()
		{
			Direction = external.Direction;
			KeyingOption = external.KeyingOption;
		}
	}
}
