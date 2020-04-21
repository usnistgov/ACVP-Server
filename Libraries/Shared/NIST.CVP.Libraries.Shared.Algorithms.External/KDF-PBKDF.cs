using System.Collections.Generic;
using System.Text.Json.Serialization;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;

namespace NIST.CVP.Libraries.Shared.Algorithms.External
{
	public class KDF_PBKDF : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("capabilities")]
		public List<Capability> Capabilities { get; set; }

		public KDF_PBKDF()
		{
			Name = "kdf-components";
			Mode = "pbkdf";
			Revision = "1.0";
		}

		public class Capability
		{
			[JsonPropertyName("iterationCount")]
			public Domain IterationCount { get; set; }

			[JsonPropertyName("hmacAlg")]
			public List<string> HMACAlgorithms { get; set; }

			[JsonPropertyName("passwordLen")]
			public Domain PasswordLength { get; set; }

			[JsonPropertyName("saltLen")]
			public Domain SaltLength { get; set; }

			[JsonPropertyName("keyLen")]
			public Domain KeyLength { get; set; }
		}
	}
}
