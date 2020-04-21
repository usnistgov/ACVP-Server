using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NIST.CVP.Libraries.Shared.Algorithms.External
{
	public class ECDSAKeyVer186_5 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("curve")]
		public List<string> Curves { get; set; }

		public ECDSAKeyVer186_5()
		{
			Name = "ECDSA";
			Mode = "keyVer";
			Revision = "FIPS186-5";
		}
	}
}
