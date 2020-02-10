using System.Collections.Generic;
using System.Text.Json.Serialization;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.External
{
	public class AES_XTS : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("direction")]
		public List<string> Direction { get; set; }

		[JsonPropertyName("keyLen")]
		public List<int> KeyLength { get; set; }

		[JsonPropertyName("payloadLen")]
		public Domain PayloadLength { get; set; }

		[JsonPropertyName("tweakMode")]
		public List<string> TweakMode { get; set; }

		public AES_XTS()
		{
			Name = "ACVP-AES-XTS";
			Revision = "1.0";
		}
	}
}
