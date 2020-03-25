using System.Collections.Generic;
using System.Text.Json.Serialization;
using NIST.CVP.Algorithms.DataTypes;

namespace NIST.CVP.Algorithms.External
{
	public class TDES_ECB : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("direction")]
		public List<string> Direction { get; set; }

		[JsonPropertyName("keyingOption")]
		public List<int> KeyingOption { get; set; }

		[JsonPropertyName("ptLen")]
		public List<int> PayloadLength { get; set; }

		public TDES_ECB()
		{
			Name = "ACVP-TDES-ECB";
			Revision = "1.0";
		}
	}
}
