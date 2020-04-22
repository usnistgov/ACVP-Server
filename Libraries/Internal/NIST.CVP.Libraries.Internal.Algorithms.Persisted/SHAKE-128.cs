using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;
using External = NIST.CVP.Libraries.Shared.Algorithms.External;

namespace NIST.CVP.Libraries.Internal.Algorithms.Persisted
{
	public class SHAKE_128 : PersistedAlgorithmBase
	{
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

		public SHAKE_128()
		{
			Name = "SHAKE-128";
			Revision = "1.0";
		}

		public SHAKE_128(External.SHAKE_128 external) : this()
		{
			InBit = external.InBit;
			InEmpty = external.InEmpty;
			Function = external.Function;
			OutBit = external.OutBit;
			OutputLength = external.OutputLength;
		}
	}
}
