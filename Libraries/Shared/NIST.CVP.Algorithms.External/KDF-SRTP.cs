using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NIST.CVP.Algorithms.External
{
	public class KDF_SRTP : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("aesKeyLength")]
		public List<int> AESKeyLengths { get; set; }

		[JsonPropertyName("supportsZeroKdr")]
		public bool SupportsZeroKDR { get; set; }

		[JsonPropertyName("kdrExponent")]
		public List<int> KDRExponents { get; set; }

		public KDF_SRTP()
		{
			Name = "kdf-components";
			Mode = "srtp";
			Revision = "1.0";
		}
	}
}
