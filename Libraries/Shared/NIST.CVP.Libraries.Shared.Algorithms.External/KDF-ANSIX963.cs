using System.Collections.Generic;
using System.Text.Json.Serialization;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;

namespace NIST.CVP.Libraries.Shared.Algorithms.External
{
	public class KDF_ANSIX963 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("hashAlg")]
		public List<string> HashAlgorithms { get; set; }

		[JsonPropertyName("fieldSize")]
		public List<int> FieldSizes { get; set; }

		[JsonPropertyName("sharedInfoLength")]
		public Domain SharedInfoLength { get; set; }

		[JsonPropertyName("keyDataLength")]
		public Domain KeyDataLength { get; set; }

		public KDF_ANSIX963()
		{
			Name = "kdf-components";
			Mode = "ansix9.63";
			Revision = "1.0";
		}
	}
}
