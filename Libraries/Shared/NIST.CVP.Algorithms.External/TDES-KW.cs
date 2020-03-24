using System.Collections.Generic;
using System.Text.Json.Serialization;
using NIST.CVP.Algorithms.DataTypes;

namespace NIST.CVP.Algorithms.External
{
	public class TDES_KW : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("direction")]
		public List<string> Direction { get; set; }

		[JsonPropertyName("kwCipher")]
		public List<string> Cipher { get; set; }

		[JsonPropertyName("payloadLen")]
		public Domain PayloadLength { get; set; }

		public TDES_KW()
		{
			Name = "ACVP-TDES-KW";
			Revision = "1.0";
		}
	}
}
