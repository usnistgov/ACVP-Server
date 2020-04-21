using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NIST.CVP.Libraries.Shared.Algorithms.External
{
	public class KAS_ECC_CDH : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("function")]
		public List<string> Functions { get; set; }

		[JsonPropertyName("curve")]
		public List<string> Curves { get; set; }

		public KAS_ECC_CDH()
		{
			Name = "KAS-ECC";
			Mode = "CDH-Component";
			Revision = "1.0";
		}
	}
}
