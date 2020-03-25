using System.Collections.Generic;
using External = NIST.CVP.Algorithms.External;

namespace NIST.CVP.Algorithms.Persisted
{
	public class RSASigGen186_2 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("capabilities")]
		public List<Capability> Capabilities { get; set; } = new List<Capability>();

		public RSASigGen186_2()
		{
			Name = "RSA";
			Mode = "sigGen";
			Revision = "186-2";
		}

		public RSASigGen186_2(External.RSASigGen186_2 external) : this()
		{
			//External doesn't use any properties...
		}

		public class Capability
		{
			[AlgorithmProperty("sigType")]
			public string SignatureType { get; set; }

			[AlgorithmProperty("modulo")]
			public List<int> Modulo { get; set; }

			[AlgorithmProperty("hashAlg")]
			public List<string> HashAlgorithms { get; set; }
		}
	}	
}
