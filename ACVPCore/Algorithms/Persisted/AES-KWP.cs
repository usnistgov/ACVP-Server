﻿using System.Collections.Generic;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.Persisted
{
	public class AES_KWP : PersistedAlgorithmBase
	{
		[AlgorithmProperty("direction")]
		public List<string> Direction { get; set; }

		[AlgorithmProperty("kwCipher")]
		public List<string> Cipher { get; set; }

		[AlgorithmProperty("key")]
		public List<int> KeyLength { get; set; }

		[AlgorithmProperty("pt")]
		public Domain PayloadLength { get; set; }

		public AES_KWP()
		{
			Name = "ACVP-AES-KWP";
			Revision = "1.0";
		}

		public AES_KWP(ACVPCore.Algorithms.External.AES_KWP external) : this()
		{
			Direction = external.Direction;
			Cipher = external.Cipher;
			KeyLength = external.KeyLength;
			PayloadLength = external.PayloadLength;
		}
	}
}