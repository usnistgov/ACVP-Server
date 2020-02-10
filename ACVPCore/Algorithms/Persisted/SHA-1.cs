using System.Collections.Generic;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.Persisted
{
	public class SHA_1 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("digestSize")]
		public List<string> DigestSize { get; } = new List<string> { "160" };

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

		public SHA_1(ACVPCore.Algorithms.External.SHA_1 external) : this()
		{
			MessageLength = external.MessageLength;
			Function = external.Function;
		}
	}
}
