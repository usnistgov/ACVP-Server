using System.Collections.Generic;
using System.Text.Json.Serialization;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.External
{
	public class AES_KWP : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("direction")]
		public List<string> Direction { get; set; }

		[JsonPropertyName("kwCipher")]
		public List<string> Cipher { get; set; }

		[JsonPropertyName("keyLen")]
		public List<int> KeyLength { get; set; }

		[JsonPropertyName("payloadLen")]
		public Domain PayloadLength { get; set; }

		public AES_KWP()
		{
			Name = "ACVP-AES-KWP";
			Revision = "1.0";
		}
	}
}
