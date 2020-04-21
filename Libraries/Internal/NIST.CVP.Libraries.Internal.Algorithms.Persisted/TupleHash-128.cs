using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;
using External = NIST.CVP.Libraries.Shared.Algorithms.External;

namespace NIST.CVP.Libraries.Internal.Algorithms.Persisted
{
	public class TupleHash_128 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("digestSize")]
		public List<int> DigestSize { get; set; }

		[AlgorithmProperty("msgLen")]
		public List<Range> MessageLength { get; set; }

		[AlgorithmProperty("outputLen")]
		public List<Range> OutputLength { get; set; }

		[AlgorithmProperty("hexCustomization")]
		public bool HexCustomization { get; set; }

		[AlgorithmProperty("xof")]
		public List<bool> XOF { get; set; }

		public TupleHash_128()
		{
			Name = "TUPLEHASH-128";
			Revision = "1.0";
		}

		public TupleHash_128(External.TupleHash_128 external) : this()
		{
			DigestSize = external.DigestSize;
			MessageLength = external.MessageLength;
			OutputLength = external.OutputLength;
			HexCustomization = external.HexCustomization;
			XOF = external.XOF;
		}
	}
}
