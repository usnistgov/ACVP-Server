using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ACVPCore.Algorithms.External
{
	public class DSASigGen186_4 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("conformances")]
		public List<string> Conformances { get; set; }

		[JsonPropertyName("capabilities")]
		public List<Capability> Capabilities { get; set; }

		public DSASigGen186_4()
		{
			Name = "DSA";
			Mode = "sigGen";
			Revision = "1.0";
		}

		public class Capability
		{
			[JsonPropertyName("l")]
			public int L { get; set; }

			[JsonPropertyName("n")]
			public int N { get; set; }

			[JsonPropertyName("hashAlg")]
			public List<string> HashAlgorithms { get; set; }
		}
	}
}
