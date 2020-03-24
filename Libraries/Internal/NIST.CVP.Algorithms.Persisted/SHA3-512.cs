using System.Collections.Generic;
using NIST.CVP.Algorithms.DataTypes;
using External = NIST.CVP.Algorithms.External;

namespace NIST.CVP.Algorithms.Persisted
{
	public class SHA3_512 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("digestSize")]
		public List<string> DigestSize { get; } = new List<string> { "512" };

		[AlgorithmProperty("inBit")]
		public bool? InBit { get; set; }

		[AlgorithmProperty("inEmpty")]
		public bool? InEmpty { get; set; }

		[AlgorithmProperty("function")]
		public List<string> Function { get; set; }

		public SHA3_512()
		{
			Name = "SHA3-512";
			Revision = "1.0";
		}

		public SHA3_512(External.SHA3_512 external) : this()
		{
			InBit = external.InBit;
			InEmpty = external.InEmpty;
			Function = external.Function;
		}
	}
}
