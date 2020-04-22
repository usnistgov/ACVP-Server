using System.Collections.Generic;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults.DSA
{
	public class DSAPQGGen_Results : AlgorithmResultsBase
	{
		public List<PassFailResult> PQGGen_ProbablePrime { get; set; }
		public List<PassFailResult> PQGGen_ProvablePrime { get; set; }
		public List<PassFailResult> PQGGen_UnverifiableG { get; set; }
		public List<PassFailResult> PQGGen_CanonicalG { get; set; }
	}
}