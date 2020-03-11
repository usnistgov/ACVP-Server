using System.Collections.Generic;

namespace ACVPCore.Algorithms.Persisted
{
	public class DeterministicECDSASigGen186_5 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("componentTest")]
		public bool? ComponentTest { get; set; }

		[AlgorithmProperty("conformances")]
		public List<string> Conformances { get; set; }

		[AlgorithmProperty("capabilities")]
		public List<Capability> Capabilities { get; set; } = new List<Capability>();

		public DeterministicECDSASigGen186_5()
		{
			Name = "DetECDSA";
			Mode = "sigGen";
			Revision = "FIPS186-5";
		}

		public DeterministicECDSASigGen186_5(ACVPCore.Algorithms.External.DeterministicECDSASigGen186_5 external) : this()
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
