using System.Collections.Generic;
using NIST.CVP.Algorithms.DataTypes;
using External = NIST.CVP.Algorithms.External;

namespace NIST.CVP.Algorithms.Persisted
{
	public class AES_CCM : PersistedAlgorithmBase
	{

		[AlgorithmProperty("key")]
		public List<int> KeyLength{ get; set; }

		[AlgorithmProperty("tag")]
		public List<int> TagLength { get; set; }

		[AlgorithmProperty("iv")]
		public Domain IVLength { get; set; }

		[AlgorithmProperty("pt")]
		public Domain PayloadLength { get; set; }

		[AlgorithmProperty("aad")]
		public Domain AADLength { get; set; }

		public AES_CCM()
		{
			Name = "ACVP-AES-CCM";
			Revision = "1.0";
		}

		public AES_CCM(External.AES_CCM external) : this()
		{
			KeyLength = external.KeyLength;
			TagLength = external.TagLength;
			IVLength = external.IVLength;
			PayloadLength = external.PayloadLength;
			AADLength = external.AADLength;
		}
	}
}
