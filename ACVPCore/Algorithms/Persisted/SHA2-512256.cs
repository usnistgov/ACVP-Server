﻿using System.Collections.Generic;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.Persisted
{
	public class SHA2_512256 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("digestSize")]
		public List<string> DigestSize { get; } = new List<string> { "512/256" };

		[AlgorithmProperty("messageLength")]
		public Domain MessageLength { get; set; }

		[AlgorithmProperty("function")]
		public List<string> Function { get; set; }

		[AlgorithmProperty("inBit")]
		public bool? InBit { get; set; }

		[AlgorithmProperty("inEmpty")]
		public bool? InEmpty { get; set; }

		public SHA2_512256()
		{
			Name = "SHA2-512/256";
			Revision = "1.0";
		}

		public SHA2_512256(ACVPCore.Algorithms.External.SHA2_512256 external) : this()
		{
			MessageLength = external.MessageLength;
			Function = external.Function;
		}
	}
}