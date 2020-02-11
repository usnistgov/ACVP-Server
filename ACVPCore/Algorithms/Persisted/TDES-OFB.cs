using System.Collections.Generic;

namespace ACVPCore.Algorithms.Persisted
{
	public class TDES_OFB : PersistedAlgorithmBase
	{
		[AlgorithmProperty("direction")]
		public List<string> Direction { get; set; }

		[AlgorithmProperty("key")]
		public List<int> KeyingOption { get; set; }

		public TDES_OFB()
		{
			Name = "ACVP-TDES-OFB";
			Revision = "1.0";
		}

		public TDES_OFB(ACVPCore.Algorithms.External.TDES_OFB external) : this()
		{
			Direction = external.Direction;
			KeyingOption = external.KeyingOption;
		}
	}
}
