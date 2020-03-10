using System.Collections.Generic;

namespace LCAVPCore.AlgorithmResults.DSA
{
	public class DSAPQGVer_Results : AlgorithmResultsBase
	{
		public List<PassFailResult> PQGVer_ProbablePrime { get; set; }
		public List<PassFailResult> PQGVer_ProvablePrime { get; set; }
		public List<PassFailResult> PQGVer_UnverifiableG { get; set; }
		public List<PassFailResult> PQGVer_CanonicalG { get; set; }
		public List<PassFailResult> PQGVer_186_2 { get; set; }
	}
}