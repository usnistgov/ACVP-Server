﻿using System.Collections.Generic;
using System.Text.Json.Serialization;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.External
{
	public class AES_ECB : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("direction")]
		public List<string> Direction { get; set; }

		[JsonPropertyName("keyLen")]
		public List<int> KeyLength { get; set; }

		[JsonPropertyName("ptLen")]
		public List<int> PayloadLength { get; set; }

		public AES_ECB()
		{
			Name = "ACVP-AES-ECB";
			Revision = "1.0";
		}
	}
}