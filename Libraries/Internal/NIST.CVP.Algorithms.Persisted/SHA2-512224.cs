﻿using System.Collections.Generic;
using NIST.CVP.Algorithms.DataTypes;
using External = NIST.CVP.Algorithms.External;

namespace NIST.CVP.Algorithms.Persisted
{
	public class SHA2_512224 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("digestSize")]
		public List<string> DigestSize { get; } = new List<string> { "512/224" };

		[AlgorithmProperty("messageLength")]
		public Domain MessageLength { get; set; }

		[AlgorithmProperty("function")]
		public List<string> Function { get; set; }

		[AlgorithmProperty("inBit")]
		public bool? InBit { get; set; }

		[AlgorithmProperty("inEmpty")]
		public bool? InEmpty { get; set; }

		public SHA2_512224()
		{
			Name = "SHA2-512/224";
			Revision = "1.0";
		}

		public SHA2_512224(External.SHA2_512224 external) : this()
		{
			MessageLength = external.MessageLength;
			Function = external.Function;
		}
	}
}