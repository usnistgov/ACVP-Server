using System.Collections.Generic;
using External = NIST.CVP.Algorithms.External;

namespace NIST.CVP.Algorithms.Persisted
{
	public class TDES_CBC : PersistedAlgorithmBase
	{
		[AlgorithmProperty("direction")]
		public List<string> Direction { get; set; }

		[AlgorithmProperty("key")]
		public List<int> KeyingOption { get; set; }

		[AlgorithmProperty("pt")]
		public List<int> PTLength { get; set; }

		public TDES_CBC()
		{
			Name = "ACVP-TDES-CBC";
			Revision = "1.0";
		}

		public TDES_CBC(External.TDES_CBC external) : this()
		{
			Direction = external.Direction;
			KeyingOption = external.KeyingOption;
			PTLength = external.PTLength;
		}
	}
}
