﻿using System.Collections.Generic;
using System.Text.Json.Serialization;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.External
{
	public class SHA2_384 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("digestSize")]
		public List<string> DigestSize { get; set; }

		[JsonPropertyName("messageLength")]
		public Domain MessageLength { get; set; }

		[JsonPropertyName("function")]
		public List<string> Function { get; set; }

		public SHA2_384()
		{
			Name = "SHA2-384";
			Revision = "1.0";
		}
	}
}