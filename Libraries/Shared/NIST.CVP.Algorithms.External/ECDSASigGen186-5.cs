using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NIST.CVP.Algorithms.External
{
	public class ECDSASigGen186_5 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("componentTest")]
		public bool? ComponentTest { get; set; }

		[JsonPropertyName("conformances")]
		public List<string> Conformances { get; set; }

		[JsonPropertyName("capabilities")]
		public List<Capability> Capabilities { get; set; }

		public ECDSASigGen186_5()
		{
			Name = "ECDSA";
			Mode = "sigGen";
			Revision = "FIPS186-5";
		}

		public class Capability
		{
			[JsonPropertyName("curve")]
			public List<string> Curves { get; set; }

			[JsonPropertyName("hashAlg")]
			public List<string> HashAlgorithms { get; set; }
		}
	}
}
