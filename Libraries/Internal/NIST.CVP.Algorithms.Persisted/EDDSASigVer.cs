using System.Collections.Generic;
using External = NIST.CVP.Algorithms.External;

namespace NIST.CVP.Algorithms.Persisted
{
	public class EDDSASigVer : PersistedAlgorithmBase
	{
		[AlgorithmProperty("curve")]
		public List<string> Curves { get; set; }

		public EDDSASigVer()
		{
			Name = "EDDSA";
			Mode = "sigVer";
			Revision = "1.0";
		}

		public EDDSASigVer(External.EDDSASigVer external) : this()
		{
			Curves = external.Curves;
		}
	}
}
