﻿using System.Collections.Generic;
using System.Text.Json.Serialization;
using NIST.CVP.Algorithms.DataTypes;

namespace NIST.CVP.Algorithms.External
{
	public class ParallelHash_256 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("digestSize")]
		public List<int> DigestSize { get; set; }

		[JsonPropertyName("msgLen")]
		public List<Range> MessageLength { get; set; }

		[JsonPropertyName("outputLen")]
		public List<Range> OutputLength { get; set; }

		[JsonPropertyName("hexCustomization")]
		public bool HexCustomization { get; set; }

		[JsonPropertyName("xof")]
		public List<bool> XOF { get; set; }

		public ParallelHash_256()
		{
			Name = "PARALLELHASH-256";
			Revision = "1.0";
		}
	}
}