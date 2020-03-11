using System.Collections.Generic;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.Persisted
{
	public class AES_GCM_SIV : PersistedAlgorithmBase
	{
		[AlgorithmProperty("direction")]
		public List<string> Direction { get; set; }

		[AlgorithmProperty("key")]
		public List<int> KeyLength { get; set; }

		[AlgorithmProperty("pt")]
		public List<int> PayloadLength { get; set; }

		[AlgorithmProperty("aad")]
		public Domain AADLength { get; set; }

		public AES_GCM_SIV()
		{
			Name = "ACVP-AES-GCM-SIV";
			Revision = "1.0";
		}

		public AES_GCM_SIV(ACVPCore.Algorithms.External.AES_GCM_SIV external) : this()
		{
			Direction = external.Direction;
			KeyLength = external.KeyLength;
			PayloadLength = external.PayloadLength;
			AADLength = external.AADLength;
		}
	}
}
