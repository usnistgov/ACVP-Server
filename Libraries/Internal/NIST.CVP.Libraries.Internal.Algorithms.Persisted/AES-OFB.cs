using System.Collections.Generic;
using External = NIST.CVP.Libraries.Shared.Algorithms.External;

namespace NIST.CVP.Libraries.Internal.Algorithms.Persisted
{
	public class AES_OFB : PersistedAlgorithmBase
	{
		[AlgorithmProperty("direction")]
		public List<string> Direction { get; set; }

		[AlgorithmProperty("key")]
		public List<int> KeyLength { get; set; }

		public AES_OFB()
		{
			Name = "ACVP-AES-OFB";
			Revision = "1.0";
		}

		public AES_OFB(External.AES_OFB external) : this()
		{
			Direction = external.Direction;
			KeyLength = external.KeyLength;
		}
	}
}
