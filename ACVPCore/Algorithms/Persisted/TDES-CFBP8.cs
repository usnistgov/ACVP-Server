using System.Collections.Generic;

namespace ACVPCore.Algorithms.Persisted
{
	public class TDES_CFBP8 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("direction")]
		public List<string> Direction { get; set; }

		[AlgorithmProperty("key")]
		public List<int> KeyingOption { get; set; }

		public TDES_CFBP8()
		{
			Name = "ACVP-TDES-CFBP8";
			Revision = "1.0";
		}

		public TDES_CFBP8(ACVPCore.Algorithms.External.TDES_CFBP8 external) : this()
		{
			Direction = external.Direction;
			KeyingOption = external.KeyingOption;
		}
	}
}
