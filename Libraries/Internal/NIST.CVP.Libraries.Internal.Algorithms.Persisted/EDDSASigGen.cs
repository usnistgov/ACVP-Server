using System.Collections.Generic;
using External = NIST.CVP.Libraries.Shared.Algorithms.External;

namespace NIST.CVP.Libraries.Internal.Algorithms.Persisted
{
	public class EDDSASigGen : PersistedAlgorithmBase
	{
		[AlgorithmProperty("curve")]
		public List<string> Curves { get; set; }

		public EDDSASigGen()
		{
			Name = "EDDSA";
			Mode = "sigGen";
			Revision = "1.0";
		}

		public EDDSASigGen(External.EDDSASigGen external) : this()
		{
			Curves = external.Curves;
		}
	}
}
