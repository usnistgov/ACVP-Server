using System.Collections.Generic;
using System.Text.Json.Serialization;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;

namespace NIST.CVP.Libraries.Shared.Algorithms.External
{
	public class KDF_ANSIX942 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("kdfType")]
		public List<string> KDFTypes { get; set; }

		[JsonPropertyName("hashAlg")]
		public List<string> HashAlgorithms { get; set; }

		[JsonPropertyName("otherInfoLen")]
		public Domain OtherInfoLength { get; set; }

		[JsonPropertyName("zzLen")]
		public Domain ZZLength { get; set; }

		[JsonPropertyName("keyLen")]
		public Domain KeyLength { get; set; }

		public KDF_ANSIX942()
		{
			Name = "kdf-components";
			Mode = "ansix9.42";
			Revision = "1.0";
		}
	}
}
