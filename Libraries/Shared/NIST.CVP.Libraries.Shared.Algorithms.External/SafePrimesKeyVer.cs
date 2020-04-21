using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NIST.CVP.Libraries.Shared.Algorithms.External
{
	public class SafePrimesKeyVer : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("safePrimeGroups")]
		public List<string> SafePrimeGroups { get; set; }

		public SafePrimesKeyVer()
		{
			Name = "safePrimes";
			Mode = "keyVer";
			Revision = "1.0";
		}
	}
}
