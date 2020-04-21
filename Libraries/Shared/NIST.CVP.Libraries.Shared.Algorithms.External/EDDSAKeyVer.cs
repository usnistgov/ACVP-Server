using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NIST.CVP.Libraries.Shared.Algorithms.External
{
	public class EDDSAKeyVer : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("curve")]
		public List<string> Curves { get; set; }

		public EDDSAKeyVer()
		{
			Name = "EDDSA";
			Mode = "keyVer";
			Revision = "1.0";
		}
	}
}
