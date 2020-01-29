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
			Name = "AES-CMAC";
			Revision = "1.0";
		}
	}

	public class Capability
	{
		[JsonPropertyName("direction")]
		public List<string> Direction { get; set; }

		[JsonPropertyName("keyLen")]
		public List<long> KeyLength { get; set; }

		[JsonPropertyName("macLen")]
		public Domain MacLength { get; set; }

		[JsonPropertyName("msgLen")]
		public Domain MessageLength { get; set; }
	}
}
