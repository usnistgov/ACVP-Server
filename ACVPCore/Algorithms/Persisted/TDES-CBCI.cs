using System.Collections.Generic;

namespace ACVPCore.Algorithms.Persisted
{
	public class TDES_CBCI : PersistedAlgorithmBase
	{
		[AlgorithmProperty("direction")]
		public List<string> Direction { get; set; }

		[AlgorithmProperty("key")]
		public List<int> KeyingOption { get; set; }

		public TDES_CBCI()
		{
			Name = "ACVP-TDES-CBCI";
			Revision = "1.0";
		}

		public TDES_CBCI(ACVPCore.Algorithms.External.TDES_CBCI external) : this()
		{
			Direction = external.Direction;
			KeyingOption = external.KeyingOption;
		}
	}
}
