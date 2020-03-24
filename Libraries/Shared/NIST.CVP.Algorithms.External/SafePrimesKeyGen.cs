using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NIST.CVP.Algorithms.External
{
	public class SafePrimesKeyGen : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("safePrimeGroups")]
		public List<string> SafePrimeGroups { get; set; }

		public SafePrimesKeyGen()
		{
			Name = "safePrimes";
			Mode = "keyGen";
			Revision = "1.0";
		}
	}
}
