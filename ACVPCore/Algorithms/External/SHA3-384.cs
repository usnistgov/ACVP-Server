using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ACVPCore.Algorithms.External
{
	public class SHA3_384 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("digestSize")]
		public List<string> DigestSize { get; set; }

		[JsonPropertyName("inBit")]
		public bool InBit { get; set; }

		[JsonPropertyName("inEmpty")]
		public bool InEmpty { get; set; }

		[JsonPropertyName("function")]
		public List<string> Function { get; set; }

		public SHA3_384()
		{
			Name = "SHA3-384";
			Revision = "1.0";
		}
	}
}
