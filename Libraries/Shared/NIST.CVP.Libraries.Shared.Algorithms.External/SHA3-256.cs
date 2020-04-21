using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NIST.CVP.Libraries.Shared.Algorithms.External
{
	public class SHA3_256 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("digestSize")]
		public List<string> DigestSize { get; set; }

		[JsonPropertyName("inBit")]
		public bool InBit { get; set; }

		[JsonPropertyName("inEmpty")]
		public bool InEmpty { get; set; }

		[JsonPropertyName("function")]
		public List<string> Function { get; set; }

		public SHA3_256()
		{
			Name = "SHA3-256";
			Revision = "1.0";
		}
	}
}
