using System.Collections.Generic;
using External = NIST.CVP.Libraries.Shared.Algorithms.External;

namespace NIST.CVP.Libraries.Internal.Algorithms.Persisted
{
	public class ECDSAKeyVer186_5 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("curve")]
		public List<string> Curves { get; set; }

		public ECDSAKeyVer186_5()
		{
			Name = "ECDSA";
			Mode = "keyVer";
			Revision = "FIPS186-5";
		}

		public ECDSAKeyVer186_5(External.ECDSAKeyVer186_5 external) : this()
		{
			Curves = external.Curves;
		}
	}
}
