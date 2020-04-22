using System.Collections.Generic;
using External = NIST.CVP.Libraries.Shared.Algorithms.External;

namespace NIST.CVP.Libraries.Internal.Algorithms.Persisted
{
	public class ECDSAKeyVer186_4 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("curve")]
		public List<string> Curves { get; set; }

		public ECDSAKeyVer186_4()
		{
			Name = "ECDSA";
			Mode = "keyVer";
			Revision = "1.0";
		}

		public ECDSAKeyVer186_4(External.ECDSAKeyVer186_4 external) : this()
		{
			Curves = external.Curves;
		}
	}
}
