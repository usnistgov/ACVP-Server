using System.Collections.Generic;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.Persisted
{
	public class SHA3_384 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("digestSize")]
		public List<string> DigestSize { get; } = new List<string> { "384" };

		[AlgorithmProperty("inBit")]
		public bool? InBit { get; set; }

		[AlgorithmProperty("inEmpty")]
		public bool? InEmpty { get; set; }

		[AlgorithmProperty("function")]
		public List<string> Function { get; set; }

		public SHA3_384()
		{
			Name = "SHA3-384";
			Revision = "1.0";
		}

		public SHA3_384(ACVPCore.Algorithms.External.SHA3_384 external) : this()
		{
			InBit = external.InBit;
			InEmpty = external.InEmpty;
			Function = external.Function;
		}
	}
}
