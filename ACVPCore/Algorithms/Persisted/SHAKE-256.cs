using System.Collections.Generic;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.Persisted
{
	public class SHAKE_256 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("digestSize")]
		public List<int> DigestSize { get; set; }

		[AlgorithmProperty("inBit")]
		public bool InBit { get; set; }

		[AlgorithmProperty("inEmpty")]
		public bool InEmpty { get; set; }

		[AlgorithmProperty("function")]
		public List<string> Function { get; set; }

		[AlgorithmProperty("outBit")]
		public bool OutBit { get; set; }

		[AlgorithmProperty("outputLen")]
		public List<Range> OutputLength { get; set; }

		public SHAKE_256()
		{
			Name = "SHAKE-256";
			Revision = "1.0";
		}

		public SHAKE_256(ACVPCore.Algorithms.External.SHAKE_256 external) : this()
		{
			DigestSize = external.DigestSize;
			InBit = external.InBit;
			InEmpty = external.InEmpty;
			Function = external.Function;
			OutBit = external.OutBit;
			OutputLength = external.OutputLength;
		}
	}
}
