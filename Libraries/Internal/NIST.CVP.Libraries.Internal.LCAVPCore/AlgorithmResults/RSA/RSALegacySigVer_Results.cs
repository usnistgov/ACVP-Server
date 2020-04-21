using System.Collections.Generic;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults.RSA
{
	public class RSALegacySigVer_Results : AlgorithmResultsBase
	{
		public List<PassFailResult> SigVer931_186_2 { get; set; } = new List<PassFailResult>();
		public List<PassFailResult> SigVerPKCS15_186_2 { get; set; } = new List<PassFailResult>();
		public List<PassFailResult> SigVerPSS_186_2 { get; set; } = new List<PassFailResult>();
	}
}