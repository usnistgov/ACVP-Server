﻿using System.Collections.Generic;

namespace ACVPCore.Algorithms.Persisted
{
	public class TDES_CFBP1 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("direction")]
		public List<string> Direction { get; set; }

		[AlgorithmProperty("key")]
		public List<int> KeyingOption { get; set; }

		public TDES_CFBP1()
		{
			Name = "ACVP-TDES-CFBP1";
			Revision = "1.0";
		}

		public TDES_CFBP1(ACVPCore.Algorithms.External.TDES_CFBP1 external) : this()
		{
			Direction = external.Direction;
			KeyingOption = external.KeyingOption;
		}
	}
}