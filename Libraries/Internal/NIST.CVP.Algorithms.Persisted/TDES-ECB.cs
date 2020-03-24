using System.Collections.Generic;
using External = NIST.CVP.Algorithms.External;

namespace NIST.CVP.Algorithms.Persisted
{
	public class TDES_ECB : PersistedAlgorithmBase
	{
		[AlgorithmProperty("direction")]
		public List<string> Direction { get; set; }

		[AlgorithmProperty("key")]
		public List<int> KeyingOption { get; set; }

		[AlgorithmProperty("pt")]
		public List<int> PayloadLength { get; set; }

		public TDES_ECB()
		{
			Name = "ACVP-TDES-ECB";
			Revision = "1.0";
		}

		public TDES_ECB(External.TDES_ECB external) : this()
		{
			Direction = external.Direction;
			KeyingOption = external.KeyingOption;
			PayloadLength = external.PayloadLength;
		}
	}
}
