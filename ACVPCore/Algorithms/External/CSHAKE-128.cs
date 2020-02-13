using System.Collections.Generic;
using System.Text.Json.Serialization;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.External
{
	public class CSHAKE_128 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("digestSize")]
		public List<int> DigestSize { get; set; }

		[JsonPropertyName("msgLen")]
		public List<Range> MessageLength { get; set; }

		[JsonPropertyName("outputLen")]
		public List<Range> OutputLength { get; set; }
		
		[JsonPropertyName("hexCustomization")]
		public bool HexCustomization { get; set; }

		public CSHAKE_128()
		{
			Name = "CSHAKE-128";
			Revision = "1.0";
		}
	}
}
