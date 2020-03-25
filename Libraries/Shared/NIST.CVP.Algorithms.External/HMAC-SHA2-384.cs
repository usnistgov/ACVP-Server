using System.Text.Json.Serialization;
using NIST.CVP.Algorithms.DataTypes;

namespace NIST.CVP.Algorithms.External
{
	public class HMAC_SHA2_384 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("macLen")]
		public Domain MacLength { get; set; }

		[JsonPropertyName("keyLen")]
		public Domain KeyLength { get; set; }

		public HMAC_SHA2_384()
		{
			Name = "HMAC-SHA2-384";
			Revision = "1.0";
		}
	}
}
