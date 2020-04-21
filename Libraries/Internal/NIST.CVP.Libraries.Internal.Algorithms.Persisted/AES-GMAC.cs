using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;
using External = NIST.CVP.Libraries.Shared.Algorithms.External;

namespace NIST.CVP.Libraries.Internal.Algorithms.Persisted
{
	public class AES_GMAC : PersistedAlgorithmBase
	{
		[AlgorithmProperty(Name = "direction")]
		public List<string> Direction { get; set; }

		[AlgorithmProperty(Name = "ivgen")]
		public string IVGen { get; set; }

		[AlgorithmProperty(Name = "ivgenmode")]
		public string IVGenMode { get; set; }

		[AlgorithmProperty(Name = "key")]
		public List<int> KeyLength { get; set; }

		[AlgorithmProperty(Name = "tag")]
		public List<int> TagLength { get; set; }

		[AlgorithmProperty(Name = "iv")]
		public Domain IVLength { get; set; }

		[AlgorithmProperty(Name = "aad")]
		public Domain AADLength { get; set; }

		public AES_GMAC()
		{
			Name = "ACVP-AES-GMAC";
			Revision = "1.0";
		}

		public AES_GMAC(External.AES_GMAC external) : this()
		{
			Direction = external.Direction;
			IVGen = external.IVGen;
			IVGenMode = external.IVGenMode;
			KeyLength = external.KeyLength;
			TagLength = external.TagLength;
			IVLength = external.IVLength;
			AADLength = external.AADLength;
		}
	}
}
