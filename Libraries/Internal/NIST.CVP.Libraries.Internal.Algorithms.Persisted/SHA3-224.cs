using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;
using External = NIST.CVP.Libraries.Shared.Algorithms.External;

namespace NIST.CVP.Libraries.Internal.Algorithms.Persisted
{
	public class SHA3_224 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("inBit")]
		public bool? InBit { get; set; }

		[AlgorithmProperty("inEmpty")]
		public bool? InEmpty { get; set; }

		[AlgorithmProperty("function")]
		public List<string> Function { get; set; }

		public SHA3_224()
		{
			Name = "SHA3-224";
			Revision = "1.0";
		}

		public SHA3_224(External.SHA3_224 external) : this()
		{
			InBit = external.InBit;
			InEmpty = external.InEmpty;
			Function = external.Function;
		}
	}
}
