using System.Collections.Generic;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults.ECDSA
{
	public class ECDSASigGen_Results : AlgorithmResultsBase
	{
		public List<PassFailResult> Results { get; set; }
	}
}