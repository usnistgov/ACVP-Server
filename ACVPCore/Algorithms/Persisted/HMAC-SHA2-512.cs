﻿using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.Persisted
{
	public class HMAC_SHA2_512 : PersistedAlgorithmBase
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

		public HMAC_SHA2_512()
		{
			Name = "HMAC-SHA2-512";
			Revision = "1.0";
		}

		public HMAC_SHA2_512(ACVPCore.Algorithms.External.HMAC_SHA2_512 external) : this()
		{
			MacLength = external.MacLength;
			KeyLength = external.KeyLength;
			//The 3 booleans are not used, that's a CAVS thing
		}
	}
}