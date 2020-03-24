using System.Collections.Generic;
using External = NIST.CVP.Algorithms.External;

namespace NIST.CVP.Algorithms.Persisted
{
	public class EDDSAKeyVer : PersistedAlgorithmBase
	{
		[AlgorithmProperty("curve")]
		public List<string> Curves { get; set; }

		public EDDSAKeyVer()
		{
			Name = "EDDSA";
			Mode = "keyVer";
			Revision = "1.0";
		}

		public EDDSAKeyVer(External.EDDSAKeyVer external) : this()
		{
			Curves = external.Curves;
		}
	}
}
