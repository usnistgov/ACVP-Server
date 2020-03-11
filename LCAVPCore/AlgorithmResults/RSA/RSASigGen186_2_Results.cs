using System.Collections.Generic;

namespace LCAVPCore.AlgorithmResults.RSA
{
	public class RSASigGen186_2_Results : AlgorithmResultsBase
	{
		public List<PassFailResult> SigGen931_186_2 { get; set; } = new List<PassFailResult>();
		public List<PassFailResult> SigGenPSS_186_2 { get; set; } = new List<PassFailResult>();
		public List<PassFailResult> SigGenPKCS15_186_2 { get; set; } = new List<PassFailResult>();
	}
}