﻿using NIST.CVP.Algorithms.DataTypes;
using External = NIST.CVP.Algorithms.External;

namespace NIST.CVP.Algorithms.Persisted
{
	public class HMAC_SHA3_512 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("mac")]
		public Domain MacLength { get; set; }

		[AlgorithmProperty("key")]
		public Domain KeyLength { get; set; }

		[AlgorithmProperty("keyLTblock")]
		public bool? KeyLessThanBlockSize { get; set; }

		[AlgorithmProperty("keyEQblock")]
		public bool? KeyEqualBlockSize { get; set; }

		[AlgorithmProperty("keyGTblock")]
		public bool? KeyGreaterThanBlockSize { get; set; }

		public HMAC_SHA3_512()
		{
			Name = "HMAC-SHA3-512";
			Revision = "1.0";
		}

		public HMAC_SHA3_512(External.HMAC_SHA3_512 external) : this()
		{
			MacLength = external.MacLength;
			KeyLength = external.KeyLength;
			//The 3 booleans are not used, that's a CAVS thing
		}
	}
}