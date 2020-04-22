using System.Collections.Generic;
using External = NIST.CVP.Libraries.Shared.Algorithms.External;

namespace NIST.CVP.Libraries.Internal.Algorithms.Persisted
{
	public class SafePrimesKeyGen : PersistedAlgorithmBase
	{
		[AlgorithmProperty("safePrimeGroups")]
		public List<string> SafePrimeGroups { get; set; }

		public SafePrimesKeyGen()
		{
			Name = "safePrimes";
			Mode = "keyGen";
			Revision = "1.0";
		}

		public SafePrimesKeyGen(External.SafePrimesKeyGen external) : this()
		{
			SafePrimeGroups = external.SafePrimeGroups;
		}
	}
}
