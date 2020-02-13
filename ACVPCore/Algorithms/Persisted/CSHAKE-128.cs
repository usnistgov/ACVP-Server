using System.Collections.Generic;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.Persisted
{
	public class CSHAKE_128 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("digestSize")]
		public List<int> DigestSize { get; set; }

		[AlgorithmProperty("msgLen")]
		public List<Range> MessageLength { get; set; }

		[AlgorithmProperty("outputLen")]
		public List<Range> OutputLength { get; set; }

		[AlgorithmProperty("hexCustomization")]
		public bool HexCustomization { get; set; }

		public CSHAKE_128()
		{
			Name = "CSHAKE-128";
			Revision = "1.0";
		}

		public CSHAKE_128(ACVPCore.Algorithms.External.CSHAKE_128 external) : this()
		{
			DigestSize = external.DigestSize;
			MessageLength = external.MessageLength;
			OutputLength = external.OutputLength;
			HexCustomization = external.HexCustomization;
		}
	}
}
