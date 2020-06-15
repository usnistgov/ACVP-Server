using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;
using External = NIST.CVP.Libraries.Shared.Algorithms.External;

namespace NIST.CVP.Libraries.Internal.Algorithms.Persisted
{
	public class ParallelHash_256 : PersistedAlgorithmBase
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

		[AlgorithmProperty("blockSize")]
		public Domain BlockSize { get; set; }

		public ParallelHash_256()
		{
			Name = "PARALLELHASH-256";
			Revision = "1.0";
		}

		public ParallelHash_256(External.ParallelHash_256 external) : this()
		{
			DigestSize = external.DigestSize;
			MessageLength = external.MessageLength;
			OutputLength = external.OutputLength;
			HexCustomization = external.HexCustomization;
			XOF = external.XOF;
			BlockSize = external.BlockSize;
		}
	}
}
