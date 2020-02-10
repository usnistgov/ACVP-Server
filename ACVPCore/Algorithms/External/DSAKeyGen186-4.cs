using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ACVPCore.Algorithms.External
{
	public class DSAKeyGen186_4 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("capabilities")]
		public List<Capability> Capabilities { get; set; }

		public DSAKeyGen186_4()
		{
			Name = "DSA";
			Mode = "keyGen";
			Revision = "1.0";
		}

		public class Capability
		{
			[JsonPropertyName("l")]
			public int L { get; set; }

			[JsonPropertyName("n")]
			public int N { get; set; }
		}
	}
}
