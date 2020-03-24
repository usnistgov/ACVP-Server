using System.Collections.Generic;
using System.Text.Json.Serialization;
using NIST.CVP.Algorithms.DataTypes;

namespace NIST.CVP.Algorithms.External
{
	public class SHAKE_256 : AlgorithmBase, IExternalAlgorithm
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

		public SHAKE_256()
		{
			Name = "SHAKE-256";
			Revision = "1.0";
		}
	}
}
