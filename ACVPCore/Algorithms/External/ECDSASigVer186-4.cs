using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ACVPCore.Algorithms.External
{
	public class ECDSASigVer186_4 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("componentTest")]
		public bool? ComponentTest { get; set; }

		[JsonPropertyName("conformances")]
		public List<string> Conformances { get; set; }

		[JsonPropertyName("capabilities")]
		public List<Capability> Capabilities { get; set; }

		public ECDSASigVer186_4()
		{
			Name = "ECDSA";
			Mode = "sigVer";
			Revision = "1.0";
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
