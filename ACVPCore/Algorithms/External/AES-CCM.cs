using System.Collections.Generic;
using System.Text.Json.Serialization;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.External
{
	public class AES_CCM : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("keyLen")]
		public List<int> KeyLength{ get; set; }

		[JsonPropertyName("tagLen")]
		public List<int> TagLength { get; set; }

		[JsonPropertyName("ivLen")]
		public Domain IVLength { get; set; }

		[JsonPropertyName("payloadLen")]
		public Domain PayloadLength { get; set; }

		[JsonPropertyName("aadLen")]
		public Domain AADLength { get; set; }

		public AES_CCM()
		{
			Name = "ACVP-AES-CCM";
			Revision = "1.0";
		}
	}
}
