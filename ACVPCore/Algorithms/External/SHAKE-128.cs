using System.Collections.Generic;
using System.Text.Json.Serialization;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.External
{
	public class SHAKE_128 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("digestSize")]
		public List<int> DigestSize { get; set; }

		[JsonPropertyName("inBit")]
		public bool InBit { get; set; }

		[JsonPropertyName("inEmpty")]
		public bool InEmpty { get; set; }
		
		[JsonPropertyName("function")]
		public List<string> Function { get; set; }

		[JsonPropertyName("outBit")]
		public bool OutBit { get; set; }

		[JsonPropertyName("outputLen")]
		public List<Range> OutputLength { get; set; }

		public SHAKE_128()
		{
			Name = "SHAKE-128";
			Revision = "1.0";
		}
	}
}
