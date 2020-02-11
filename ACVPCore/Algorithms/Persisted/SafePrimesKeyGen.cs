using System.Collections.Generic;

namespace ACVPCore.Algorithms.Persisted
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

		public SafePrimesKeyGen(ACVPCore.Algorithms.External.SafePrimesKeyGen external) : this()
		{
			SafePrimeGroups = external.SafePrimeGroups;
		}
	}
}
