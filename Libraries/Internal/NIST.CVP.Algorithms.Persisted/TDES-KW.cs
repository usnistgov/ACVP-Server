using System.Collections.Generic;
using NIST.CVP.Algorithms.DataTypes;
using External = NIST.CVP.Algorithms.External;

namespace NIST.CVP.Algorithms.Persisted
{
	public class TDES_KW : PersistedAlgorithmBase
	{
		[AlgorithmProperty("direction")]
		public List<string> Direction { get; set; }

		[AlgorithmProperty("kwCipher")]
		public List<string> Cipher { get; set; }

		[AlgorithmProperty("pt")]
		public Domain PayloadLength { get; set; }

		public TDES_KW()
		{
			Name = "ACVP-TDES-KW";
			Revision = "1.0";
		}

		public TDES_KW(External.TDES_KW external) : this()
		{
			Direction = external.Direction;
			Cipher = external.Cipher;
			PayloadLength = external.PayloadLength;
		}
	}
}
