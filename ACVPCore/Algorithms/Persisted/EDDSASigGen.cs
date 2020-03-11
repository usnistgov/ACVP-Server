using System.Collections.Generic;

namespace ACVPCore.Algorithms.Persisted
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

		public EDDSASigGen(ACVPCore.Algorithms.External.EDDSASigGen external) : this()
		{
			Curves = external.Curves;
		}
	}
}
