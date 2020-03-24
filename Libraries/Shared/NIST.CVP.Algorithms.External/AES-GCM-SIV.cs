using System.Collections.Generic;
using System.Text.Json.Serialization;
using NIST.CVP.Algorithms.DataTypes;

namespace NIST.CVP.Algorithms.External
{
	public class AES_GCM_SIV : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("direction")]
		public List<string> Direction { get; set; }

		[JsonPropertyName("keyLen")]
		public List<int> KeyLength { get; set; }
		
		[JsonPropertyName("payloadLen")]
		public List<int> PayloadLength { get; set; }

		[JsonPropertyName("aadLen")]
		public Domain AADLength { get; set; }

		public AES_GCM_SIV()
		{
			Name = "ACVP-AES-GCM-SIV";
			Revision = "1.0";
		}
	}
}
