using System.Collections.Generic;
using System.Text.Json.Serialization;
using NIST.CVP.Algorithms.DataTypes;

namespace NIST.CVP.Algorithms.External
{
	public class AES_CBC_CS2 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("direction")]
		public List<string> Direction { get; set; }

		[JsonPropertyName("keyLen")]
		public List<int> KeyLength{ get; set; }

		[JsonPropertyName("payloadLen")]
		public Domain PayloadLength { get; set; }

		public AES_CBC_CS2()
		{
			Name = "ACVP-AES-CBC-CS2";
			Revision = "1.0";
		}
	}
}
