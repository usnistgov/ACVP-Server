using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NIST.CVP.Algorithms.External
{
	public class EDDSASigGen : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("curve")]
		public List<string> Curves { get; set; }

		public EDDSASigGen()
		{
			Name = "EDDSA";
			Mode = "sigGen";
			Revision = "1.0";
		}
	}
}
