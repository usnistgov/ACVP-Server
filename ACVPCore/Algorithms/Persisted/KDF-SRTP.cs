﻿using System.Collections.Generic;

namespace ACVPCore.Algorithms.Persisted
{
	public class KDF_SRTP : PersistedAlgorithmBase
	{
		[AlgorithmProperty("aesKeyLength")]
		public List<int> AESKeyLengths { get; set; }

		[AlgorithmProperty("supportsZeroKdr")]
		public bool SupportsZeroKDR { get; set; }

		[AlgorithmProperty("kdrExponent")]
		public List<int> KDRExponents { get; set; }

		public KDF_SRTP()
		{
			Name = "kdf-components";
			Mode = "srtp";
			Revision = "1.0";
		}

		public KDF_SRTP(ACVPCore.Algorithms.External.KDF_SRTP external) : this()
		{
			AESKeyLengths = external.AESKeyLengths;
			SupportsZeroKDR = external.SupportsZeroKDR;
			KDRExponents = external.KDRExponents;
		}
	}

	
}