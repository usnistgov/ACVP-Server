using System.Collections.Generic;
using NIST.CVP.Algorithms.DataTypes;
using External = NIST.CVP.Algorithms.External;

namespace NIST.CVP.Algorithms.Persisted
{
	public class AES_XPN : PersistedAlgorithmBase
	{
		[AlgorithmProperty("direction")]
		public List<string> Direction { get; set; }

		[AlgorithmProperty("key")]
		public List<int> KeyLength { get; set; }

		[AlgorithmProperty("pt")]
		public Domain PayloadLength { get; set; }

		[AlgorithmProperty("aad")]
		public Domain AADLength { get; set; }

		[AlgorithmProperty("tag")]
		public List<int> TagLength { get; set; }

		[AlgorithmProperty("ivgen")]
		public string IVGen { get; set; }

		[AlgorithmProperty("ivgenmode")]
		public string IVGenMode { get; set; }

		[AlgorithmProperty("saltGen")]
		public string SaltGeneration { get; set; }


		public AES_XPN()
		{
			Name = "ACVP-AES-XPN";
			Revision = "1.0";
		}

		public AES_XPN(External.AES_XPN external) : this()
		{
			Direction = external.Direction;
			IVGen = external.IVGen;
			IVGenMode = external.IVGenMode;
			KeyLength = external.KeyLength;
			TagLength = external.TagLength;
			PayloadLength = external.PayloadLength;
			AADLength = external.AADLength;
			SaltGeneration = external.SaltGeneration;
		}
	}
}
