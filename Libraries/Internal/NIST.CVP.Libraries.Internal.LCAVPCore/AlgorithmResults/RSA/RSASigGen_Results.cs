using System.Collections.Generic;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults.RSA
{
	public class RSASigGen_Results : AlgorithmResultsBase
	{
		public List<PassFailResult> SigGen931 { get; set; } = new List<PassFailResult>();
		public List<PassFailResult> SigGenPKCS15 { get; set; } = new List<PassFailResult>();
		public List<PassFailResult> SigGenPSS { get; set; } = new List<PassFailResult>();
	}
}