using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NIST.CVP.Algorithms.External
{
	public class ECDSAKeyVer186_4 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("curve")]
		public List<string> Curves { get; set; }

		public ECDSAKeyVer186_4()
		{
			Name = "ECDSA";
			Mode = "keyVer";
			Revision = "1.0";
		}
	}
}
