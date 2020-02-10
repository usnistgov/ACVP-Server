using System.Collections.Generic;

namespace ACVPCore.Algorithms.Persisted
{
	public class ECDSAKeyVer186_4 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("curve")]
		public List<string> Curves { get; set; }

		public ECDSAKeyVer186_4()
		{
			Name = "ECDSA";
			Mode = "keyGen";
			Revision = "1.0";
		}

		public ECDSAKeyVer186_4(ACVPCore.Algorithms.External.ECDSAKeyVer186_4 external) : this()
		{
			Curves = external.Curves;
		}
	}
}
