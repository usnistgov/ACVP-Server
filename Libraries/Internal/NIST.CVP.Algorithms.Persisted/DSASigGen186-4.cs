using System.Collections.Generic;
using External = NIST.CVP.Algorithms.External;

namespace NIST.CVP.Algorithms.Persisted
{
	public class DSASigGen186_4 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("conformances")]
		public List<string> Conformances { get; set; }

		[AlgorithmProperty("capabilities")]
		public List<Capability> Capabilities { get; set; } = new List<Capability>();

		public DSASigGen186_4()
		{
			Name = "DSA";
			Mode = "sigGen";
			Revision = "1.0";
		}

		public DSASigGen186_4(External.DSASigGen186_4 external) : this()
		{
			Conformances = external.Conformances;

			foreach (var capability in external.Capabilities)
			{
				Capabilities.Add(new Capability
				{
					L = capability.L,
					N = capability.N,
					HashAlgorithms = capability.HashAlgorithms
				});
			}
		}

		public class Capability
		{

			[AlgorithmProperty("l")]
			public int L { get; set; }

			[AlgorithmProperty("n")]
			public int N { get; set; }

			[AlgorithmProperty("hashAlg")]
			public List<string> HashAlgorithms { get; set; }
		}
	}

	
}
