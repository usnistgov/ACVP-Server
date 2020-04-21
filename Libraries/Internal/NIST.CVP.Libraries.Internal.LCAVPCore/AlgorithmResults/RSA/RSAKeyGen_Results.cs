using System.Collections.Generic;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults.RSA
{
	public class RSAKeyGen_Results : AlgorithmResultsBase
	{
		public List<PassFailResult> Results { get; set; } = new List<PassFailResult>();
	}
}