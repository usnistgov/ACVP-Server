using System.Collections.Generic;
using NIST.CVP.Algorithms.DataTypes;
using External = NIST.CVP.Algorithms.External;

namespace NIST.CVP.Algorithms.Persisted
{
	public class SHA_1 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("messageLength")]
		public Domain MessageLength { get; set; }

		[AlgorithmProperty("function")]
		public List<string> Function { get; set; }

		[AlgorithmProperty("inBit")]
		public bool? InBit { get; set; }

		[AlgorithmProperty("inEmpty")]
		public bool? InEmpty { get; set; }

		public SHA_1()
		{
			Name = "SHA-1";
			Revision = "1.0";
		}

		public SHA_1(External.SHA_1 external) : this()
		{
			MessageLength = external.MessageLength;
			Function = external.Function;
		}
	}
}
