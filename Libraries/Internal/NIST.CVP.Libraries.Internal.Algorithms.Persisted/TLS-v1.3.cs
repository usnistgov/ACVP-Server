using System.Collections.Generic;
using External = NIST.CVP.Libraries.Shared.Algorithms.External;

namespace NIST.CVP.Libraries.Internal.Algorithms.Persisted
{
	public class TLS_v1_3 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("hashAlg")]
		public List<string> HashAlgorithms { get; set; }

		public TLS_v1_3()
		{
			Name = "kdf-components";
			Mode = "tls";
			Revision = "1.0";
		}

		public TLS_v1_3(External.TLS_v1_3 external) : this()
		{
			HashAlgorithms = external.HashAlgorithms;
		}
	}

	
}
