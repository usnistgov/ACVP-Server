using System.Collections.Generic;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.Persisted
{
	public class KMAC_256 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("digestSize")]
		public List<int> DigestSize { get; set; }

		[AlgorithmProperty("msgLen")]
		public List<Range> MessageLength { get; set; }

		[AlgorithmProperty("macLen")]
		public List<Range> MacLength { get; set; }

		[AlgorithmProperty("keyLen")]
		public List<Range> KeyLength { get; set; }

		[AlgorithmProperty("hexCustomization")]
		public bool HexCustomization { get; set; }

		[AlgorithmProperty("xof")]
		public List<bool> XOF { get; set; }

		public KMAC_256()
		{
			Name = "KMAC-256";
			Revision = "1.0";
		}

		public KMAC_256(ACVPCore.Algorithms.External.KMAC_256 external) : this()
		{
			DigestSize = external.DigestSize;
			MessageLength = external.MessageLength;
			HexCustomization = external.HexCustomization;
			MacLength = external.MacLength;
			KeyLength = external.KeyLength;
			XOF = external.XOF;
		}
	}
}
