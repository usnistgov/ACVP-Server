using System.Collections.Generic;
using External = NIST.CVP.Algorithms.External;

namespace NIST.CVP.Algorithms.Persisted
{
	public class ECDSASigVer186_4 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("componentTest")]
		public bool? ComponentTest { get; set; }

		[AlgorithmProperty("conformances")]
		public List<string> Conformances { get; set; }

		[AlgorithmProperty("capabilities")]
		public List<Capability> Capabilities { get; set; } = new List<Capability>();

		public ECDSASigVer186_4()
		{
			Name = "ECDSA";
			Mode = "sigVer";
			Revision = "1.0";
		}

		public ECDSASigVer186_4(External.ECDSASigVer186_4 external) : this()
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
