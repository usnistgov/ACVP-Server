﻿using System.Collections.Generic;

namespace ACVPCore.Algorithms.Persisted
{
	public class TDES_OFBI : PersistedAlgorithmBase
	{
		[AlgorithmProperty("direction")]
		public List<string> Direction { get; set; }

		[AlgorithmProperty("key")]
		public List<int> KeyingOption { get; set; }

		public TDES_OFBI()
		{
			Name = "ACVP-TDES-OFBI";
			Revision = "1.0";
		}

		public TDES_OFBI(ACVPCore.Algorithms.External.TDES_OFBI external) : this()
		{
			Direction = external.Direction;
			KeyingOption = external.KeyingOption;
		}
	}
}