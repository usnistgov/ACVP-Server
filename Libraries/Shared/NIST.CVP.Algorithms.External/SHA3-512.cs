﻿using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NIST.CVP.Algorithms.External
{
	public class SHA3_512 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("digestSize")]
		public List<string> DigestSize { get; set; }

		[JsonPropertyName("inBit")]
		public bool InBit { get; set; }

		[JsonPropertyName("inEmpty")]
		public bool InEmpty { get; set; }

		[JsonPropertyName("function")]
		public List<string> Function { get; set; }

		public SHA3_512()
		{
			Name = "SHA3-512";
			Revision = "1.0";
		}
	}
}