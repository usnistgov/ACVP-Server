using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;
using External = NIST.CVP.Libraries.Shared.Algorithms.External;

namespace NIST.CVP.Libraries.Internal.Algorithms.Persisted
{
	public class AES_XTS : PersistedAlgorithmBase
	{
		[AlgorithmProperty("direction")]
		public List<string> Direction { get; set; }

		[AlgorithmProperty("key")]
		public List<int> KeyLength { get; set; }

		[AlgorithmProperty("pt")]
		public Domain PayloadLength { get; set; }

		[AlgorithmProperty("tweakMode")]
		public List<string> TweakMode { get; set; }

		[AlgorithmProperty("blockSize")]
		public List<string> BlockSize { get; set; }

		[AlgorithmProperty("maxLen")]
		public int? MaxLength { get; set; }

		public AES_XTS()
		{
			Name = "ACVP-AES-XTS";
			Revision = "1.0";
		}

		public AES_XTS(External.AES_XTS external) : this()
		{
			Direction = external.Direction;
			KeyLength = external.KeyLength;
			PayloadLength = external.PayloadLength;
			TweakMode = external.TweakMode;
			//BlockSize and MaxLength not used by 1.0
		}
	}
}
