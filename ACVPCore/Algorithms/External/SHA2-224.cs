﻿using System.Text.Json.Serialization;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.External
{
	public class SHA2_224 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("messageLength")]
		public Domain MessageLength { get; set; }

		public SHA2_224()
		{
			Name = "SHA2-224";
			Revision = "1.0";
		}
	}
}