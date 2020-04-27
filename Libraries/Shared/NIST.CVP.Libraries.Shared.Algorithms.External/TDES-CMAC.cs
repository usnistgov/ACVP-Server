using System.Collections.Generic;
using System.Text.Json.Serialization;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;

namespace NIST.CVP.Libraries.Shared.Algorithms.External
{
	public class TDES_CMAC : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("capabilities")]
		public List<Capability> Capabilities { get; set; }

		public TDES_CMAC()
		{
			Name = "CMAC-TDES";
			Revision = "1.0";
		}

		public class Capability
		{
			[JsonPropertyName("direction")]
			public List<string> Direction { get; set; }

			[JsonPropertyName("keyingOption")]
			public List<int> KeyingOption { get; set; }

			[JsonPropertyName("macLen")]
			public Domain MacLength { get; set; }

			[JsonPropertyName("msgLen")]
			public Domain MessageLength { get; set; }
		}
	}
}
