using System.Collections.Generic;
using System.Text.Json.Serialization;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;

namespace NIST.CVP.Libraries.Shared.Algorithms.External
{
	public class KDF : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("capabilities")]
		public List<Capability> Capabilities { get; set; }

		public KDF()
		{
			Name = "KDF";
			Revision = "1.0";
		}

		public class Capability
		{
			[JsonPropertyName("kdfMode")]
			public string KDFMode { get; set; }

			[JsonPropertyName("macMode")]
			public List<string> MacMode { get; set; }

			[JsonPropertyName("supportedLengths")]
			public Domain SupportedLengths { get; set; }

			[JsonPropertyName("fixedDataOrder")]
			public List<string> FixedDataOrder { get; set; }

			[JsonPropertyName("counterLength")]
			public List<int> CounterLength { get; set; }

			[JsonPropertyName("supportsEmptyIv")]
			public bool SupportsEmptyIV { get; set; }
		}
	}
}
