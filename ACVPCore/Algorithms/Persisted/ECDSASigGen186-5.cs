﻿using System.Collections.Generic;

namespace ACVPCore.Algorithms.Persisted
{
	public class ECDSASigGen186_5 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("componentTest")]
		public bool? ComponentTest { get; set; }

		[AlgorithmProperty("conformances")]
		public List<string> Conformances { get; set; }

		[AlgorithmProperty("capabilities")]
		public List<Capability> Capabilities { get; set; } = new List<Capability>();

		public ECDSASigGen186_5()
		{
			Name = "ECDSA";
			Mode = "sigGen";
			Revision = "FIPS186-5";
		}

		public ECDSASigGen186_5(ACVPCore.Algorithms.External.ECDSASigGen186_5 external) : this()
		{
			ComponentTest = external.ComponentTest;
			Conformances = external.Conformances;
			foreach (var capability in external.Capabilities)
			{
				Capabilities.Add(new Capability
				{
					Curves = capability.Curves,
					HashAlgorithms = capability.HashAlgorithms
				});
			}
		}

		public class Capability
		{

			[AlgorithmProperty("curve")]
			public List<string> Curves { get; set; }

			[AlgorithmProperty("hashAlg")]
			public List<string> HashAlgorithms { get; set; }
		}
	}

	
}