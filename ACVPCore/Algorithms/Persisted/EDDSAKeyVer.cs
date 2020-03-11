using System.Collections.Generic;

namespace ACVPCore.Algorithms.Persisted
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

		public EDDSAKeyVer(ACVPCore.Algorithms.External.EDDSAKeyVer external) : this()
		{
			Curves = external.Curves;
		}
	}
}
