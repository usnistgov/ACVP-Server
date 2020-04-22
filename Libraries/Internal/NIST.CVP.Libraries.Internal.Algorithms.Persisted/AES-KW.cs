using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;
using External = NIST.CVP.Libraries.Shared.Algorithms.External;

namespace NIST.CVP.Libraries.Internal.Algorithms.Persisted
{
	public class AES_KW : PersistedAlgorithmBase
	{
		[AlgorithmProperty("direction")]
		public List<string> Direction { get; set; }

		[AlgorithmProperty("kwCipher")]
		public List<string> Cipher { get; set; }

		[AlgorithmProperty("key")]
		public List<int> KeyLength { get; set; }

		[AlgorithmProperty("pt")]
		public Domain PayloadLength { get; set; }

		public AES_KW()
		{
			Name = "ACVP-AES-KW";
			Revision = "1.0";
		}

		public AES_KW(External.AES_KW external) : this()
		{
			Direction = external.Direction;
			Cipher = external.Cipher;
			KeyLength = external.KeyLength;
			PayloadLength = external.PayloadLength;
		}
	}
}
