using System.Collections.Generic;
using External = NIST.CVP.Algorithms.External;

namespace NIST.CVP.Algorithms.Persisted
{
	public class KDF_SSH : PersistedAlgorithmBase
	{
		[AlgorithmProperty("cipher")]
		public List<string> Ciphers { get; set; }

		[AlgorithmProperty("hashAlg")]
		public List<string> HashAlgorithms { get; set; }

		public KDF_SSH()
		{
			Name = "kdf-components";
			Mode = "ssh";
			Revision = "1.0";
		}

		public KDF_SSH(External.KDF_SSH external) : this()
		{
			Ciphers = external.Ciphers;
			HashAlgorithms = external.HashAlgorithms;
		}
	}

	
}
