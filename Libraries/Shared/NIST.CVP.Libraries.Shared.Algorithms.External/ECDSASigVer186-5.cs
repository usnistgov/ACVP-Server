using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NIST.CVP.Libraries.Shared.Algorithms.External
{
	public class ECDSASigVer186_5 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("conformances")]
		public List<string> Conformances { get; set; }

		[JsonPropertyName("capabilities")]
		public List<Capability> Capabilities { get; set; }

		public ECDSASigVer186_5()
		{
			Name = "ECDSA";
			Mode = "sigVer";
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
