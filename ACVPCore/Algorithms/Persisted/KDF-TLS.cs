using System.Collections.Generic;

namespace ACVPCore.Algorithms.Persisted
{
	public class KDF_TLS : PersistedAlgorithmBase
	{
		[AlgorithmProperty("tlsVersion")]
		public List<string> TLSVersions { get; set; }

		[AlgorithmProperty("hashAlg")]
		public List<string> HashAlgorithms { get; set; }

		public KDF_TLS()
		{
			Name = "kdf-components";
			Mode = "tls";
			Revision = "1.0";
		}

		public KDF_TLS(ACVPCore.Algorithms.External.KDF_TLS external) : this()
		{
			TLSVersions = external.TLSVersions;
			HashAlgorithms = external.HashAlgorithms;
		}
	}

	
}
