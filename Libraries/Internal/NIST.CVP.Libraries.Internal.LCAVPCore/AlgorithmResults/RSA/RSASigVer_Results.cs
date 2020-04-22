using System.Collections.Generic;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults.RSA
{
	public class RSASigVer_Results : AlgorithmResultsBase
	{
		public List<PassFailResult> SigVer931 { get; set; } = new List<PassFailResult>();
		public List<PassFailResult> SigVerPKCS15 { get; set; } = new List<PassFailResult>();
		public List<PassFailResult> SigVerPSS { get; set; } = new List<PassFailResult>();
	}
}