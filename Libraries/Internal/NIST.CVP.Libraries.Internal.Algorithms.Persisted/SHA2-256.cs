using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;
using External = NIST.CVP.Libraries.Shared.Algorithms.External;

namespace NIST.CVP.Libraries.Internal.Algorithms.Persisted
{
	public class SHA2_256 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("messageLength")]
		public Domain MessageLength { get; set; }

		[AlgorithmProperty("function")]
		public List<string> Function { get; set; }

		[AlgorithmProperty("inBit")]
		public bool? InBit { get; set; }

		[AlgorithmProperty("inEmpty")]
		public bool? InEmpty { get; set; }

		public SHA2_256()
		{
			Name = "SHA2-256";
			Revision = "1.0";
		}

		public SHA2_256(External.SHA2_256 external) : this()
		{
			MessageLength = external.MessageLength;
			Function = external.Function;
		}
	}
}
