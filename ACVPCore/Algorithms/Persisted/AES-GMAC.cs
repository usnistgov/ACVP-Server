using System.Collections.Generic;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.Persisted
{
	public class AES_GMAC : PersistedAlgorithmBase
	{
		[AlgorithmProperty(Name = "direction")]
		public List<string> Direction { get; set; }

		[AlgorithmProperty(Name = "ivGen")]
		public string IVGen { get; set; }

		[AlgorithmProperty(Name = "ivGenMode")]
		public string IVGenMode { get; set; }

		[AlgorithmProperty(Name = "key")]
		public List<int> KeyLength { get; set; }

		[AlgorithmProperty(Name = "tagLen")]
		public List<int> TagLength { get; set; }

		[AlgorithmProperty(Name = "ivLen")]
		public Domain IVLength { get; set; }

		[AlgorithmProperty(Name = "aadLen")]
		public Domain AADLength { get; set; }

		public AES_GMAC()
		{
			Name = "ACVP-AES-GMAC";
			Revision = "1.0";
		}

		public AES_GMAC(ACVPCore.Algorithms.External.AES_GMAC external) : this()
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
