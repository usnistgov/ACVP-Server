﻿using System.Collections.Generic;
using System.Text.Json.Serialization;
using NIST.CVP.Algorithms.DataTypes;

namespace NIST.CVP.Algorithms.External
{
	public class SHA2_512256 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("digestSize")]
		public List<string> DigestSize { get; set; }

		[JsonPropertyName("messageLength")]
		public Domain MessageLength { get; set; }

		[JsonPropertyName("function")]
		public List<string> Function { get; set; }

		public SHA2_512256()
		{
			Name = "SHA2-512/256";
			Revision = "1.0";
		}
	}
}