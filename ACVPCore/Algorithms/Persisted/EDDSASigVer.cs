using System.Collections.Generic;

namespace ACVPCore.Algorithms.Persisted
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

		public EDDSASigVer(ACVPCore.Algorithms.External.EDDSASigVer external) : this()
		{
			Curves = external.Curves;
		}
	}
}
