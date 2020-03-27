using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NIST.CVP.Algorithms.External
{
	public class EDDSASigVer : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("curve")]
		public List<string> Curves { get; set; }

		public EDDSASigVer()
		{
			Name = "EDDSA";
			Mode = "sigVer";
			Revision = "1.0";
		}
	}
}
