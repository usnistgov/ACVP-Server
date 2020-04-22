using System.Collections.Generic;
using System.Text.Json.Serialization;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;

namespace NIST.CVP.Libraries.Shared.Algorithms.External
{
	public class AES_CBC_CS3 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("direction")]
		public List<string> Direction { get; set; }

		[JsonPropertyName("keyLen")]
		public List<int> KeyLength{ get; set; }

		[JsonPropertyName("payloadLen")]
		public Domain PayloadLength { get; set; }

		public AES_CBC_CS3()
		{
			Name = "ACVP-AES-CBC-CS3";
			Revision = "1.0";
		}
	}
}
