﻿using System.Collections.Generic;

namespace ACVPCore.Algorithms.Persisted
{
	public class DSAKeyGen186_4 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("capabilities")]
		public List<Capability> Capabilities { get; set; } = new List<Capability>();

		public DSAKeyGen186_4()
		{
			Name = "DSA";
			Mode = "keyGen";
			Revision = "1.0";
		}

		public DSAKeyGen186_4(ACVPCore.Algorithms.External.DSAKeyGen186_4 external) : this()
		{
			foreach (var capability in external.Capabilities)
			{
				Capabilities.Add(new Capability
				{
					L = capability.L,
					N = capability.N
				});
			}
		}

		public class Capability
		{

			[AlgorithmProperty("l")]
			public int L { get; set; }

			[AlgorithmProperty("n")]
			public int N { get; set; }
		}
	}

	
}