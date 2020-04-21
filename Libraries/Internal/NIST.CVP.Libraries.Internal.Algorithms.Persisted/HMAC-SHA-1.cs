using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;
using External = NIST.CVP.Libraries.Shared.Algorithms.External;

namespace NIST.CVP.Libraries.Internal.Algorithms.Persisted
{
	public class HMAC_SHA_1 : PersistedAlgorithmBase
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

		public HMAC_SHA_1()
		{
			Name = "HMAC-SHA-1";
			Revision = "1.0";
		}

		public HMAC_SHA_1(External.HMAC_SHA_1 external) : this()
		{
			MacLength = external.MacLength;
			KeyLength = external.KeyLength;
			//The 3 booleans are not used, that's a CAVS thing
		}
	}
}
