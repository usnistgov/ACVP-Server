using System.Collections.Generic;

namespace ACVPCore.Algorithms.Persisted
{
	public class SafePrimesKeyVer : PersistedAlgorithmBase
	{
		[AlgorithmProperty("safePrimeGroups")]
		public List<string> SafePrimeGroups { get; set; }

		public SafePrimesKeyVer()
		{
			Name = "safePrimes";
			Mode = "keyVer";
			Revision = "1.0";
		}

		public SafePrimesKeyVer(ACVPCore.Algorithms.External.SafePrimesKeyVer external) : this()
		{
			SafePrimeGroups = external.SafePrimeGroups;
		}
	}
}
