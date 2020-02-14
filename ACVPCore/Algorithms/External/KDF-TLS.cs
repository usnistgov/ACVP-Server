using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ACVPCore.Algorithms.External
{
	public class KDF_TLS : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("tlsVersion")]
		public List<string> TLSVersions { get; set; }

		[JsonPropertyName("hashAlg")]
		public List<string> HashAlgorithms { get; set; }

		public KDF_TLS()
		{
			Name = "kdf-components";
			Mode = "tls";
			Revision = "1.0";
		}
	}
}
