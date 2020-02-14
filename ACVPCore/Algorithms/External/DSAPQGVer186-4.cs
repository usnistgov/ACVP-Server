using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ACVPCore.Algorithms.External
{
	public class DSAPQGVer186_4 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("capabilities")]
		public List<Capability> Capabilities { get; set; }

		public DSAPQGVer186_4()
		{
			Name = "DSA";
			Mode = "pqgVer";
			Revision = "1.0";
		}

		public class Capability
		{
			[JsonPropertyName("pqGen")]
			public List<string> PQGen { get; set; }

			[JsonPropertyName("gGen")]
			public List<string> GGen { get; set; }

			[JsonPropertyName("l")]
			public int L { get; set; }

			[JsonPropertyName("n")]
			public int N { get; set; }

			[JsonPropertyName("hashAlg")]
			public List<string> HashAlgorithms { get; set; }
		}
	}
}
