using System.Collections.Generic;
using External = NIST.CVP.Libraries.Shared.Algorithms.External;

namespace NIST.CVP.Libraries.Internal.Algorithms.Persisted
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

		public SafePrimesKeyVer(External.SafePrimesKeyVer external) : this()
		{
			SafePrimeGroups = external.SafePrimeGroups;
		}
	}
}
