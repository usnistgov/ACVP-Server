using System.Collections.Generic;
using System.Text.Json.Serialization;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.External
{
	public class AES_CMAC : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("capabilities")]
		public List<Capability> Capabilities { get; set; }

		public AES_CMAC()
		{
			Name = "CMAC-AES";
			Revision = "1.0";
		}

		public class Capability
		{
			[JsonPropertyName("direction")]
			public List<string> Direction { get; set; }

			[JsonPropertyName("key")]
			public List<int> KeyLength { get; set; }

			[JsonPropertyName("macLen")]
			public Domain MacLength { get; set; }

			[JsonPropertyName("msgLen")]
			public Domain MessageLength { get; set; }
		}
	}
}
