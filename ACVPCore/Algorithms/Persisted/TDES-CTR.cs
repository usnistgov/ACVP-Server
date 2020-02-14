using System.Collections.Generic;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.Persisted
{
	public class TDES_CTR : PersistedAlgorithmBase
	{
		[AlgorithmProperty("direction")]
		public List<string> Direction { get; set; }

		[AlgorithmProperty("key")]
		public List<int> KeyingOption { get; set; }

		[AlgorithmProperty("payloadLen")]
		public Domain PayloadLength { get; set; }

		[AlgorithmProperty("overflowCounter")]
		public bool? OverflowCounter { get; set; }

		[AlgorithmProperty("incrementalCounter")]
		public bool? IncrementalCounter { get; set; }

		[AlgorithmProperty("ctrSource")]
		public List<string> CounterSource { get; set; }

		public TDES_CTR()
		{
			Name = "ACVP-TDES-CTR";
			Revision = "1.0";
		}

		public TDES_CTR(ACVPCore.Algorithms.External.TDES_CTR external) : this()
		{
			Direction = external.Direction;
			KeyingOption = external.KeyingOption;
			PayloadLength = external.PayloadLength;
			OverflowCounter = external.OverflowCounter;
			IncrementalCounter = external.IncrementalCounter;
			//CounterSource no longer used
		}
	}
}
