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
			Name = "TLS-v1.3";
			Mode = "KDF";
			Revision = "RFC8446";
		}

		public TLS_v1_3(External.TLS_v1_3 external) : this()
		{
			HashAlgorithms = external.HashAlgorithms;
		}
	}

	
}
