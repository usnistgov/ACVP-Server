using System.Collections.Generic;
using External = NIST.CVP.Libraries.Shared.Algorithms.External;

namespace NIST.CVP.Libraries.Internal.Algorithms.Persisted
{
	public class DSAPQGVer186_4 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("capabilities")]
		public List<Capability> Capabilities { get; set; } = new List<Capability>();

		public DSAPQGVer186_4()
		{
			Name = "DSA";
			Mode = "pqgVer";
			Revision = "1.0";
		}

		public DSAPQGVer186_4(External.DSAPQGVer186_4 external) : this()
		{
			foreach (var capability in external.Capabilities)
			{
				Capabilities.Add(new Capability
				{
					PQGen = capability.PQGen,
					GGen = capability.GGen,
					L = capability.L,
					N = capability.N,
					HashAlgorithms = capability.HashAlgorithms
				});
			}
		}

		public class Capability
		{
			[AlgorithmProperty("pqGen")]
			public List<string> PQGen { get; set; }

			[AlgorithmProperty("gGen")]
			public List<string> GGen { get; set; }

			[AlgorithmProperty("l")]
			public int L { get; set; }

			[AlgorithmProperty("n")]
			public int N { get; set; }

			[AlgorithmProperty("hashAlg")]
			public List<string> HashAlgorithms { get; set; }
		}
	}

	
}
