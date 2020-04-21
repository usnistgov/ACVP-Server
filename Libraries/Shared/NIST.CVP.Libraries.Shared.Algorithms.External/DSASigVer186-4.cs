using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NIST.CVP.Libraries.Shared.Algorithms.External
{
	public class DSASigVer186_4 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("conformances")]
		public List<string> Conformances { get; set; }

		[JsonPropertyName("capabilities")]
		public List<Capability> Capabilities { get; set; }

		public DSASigVer186_4()
		{
			Name = "DSA";
			Mode = "sigVer";
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
