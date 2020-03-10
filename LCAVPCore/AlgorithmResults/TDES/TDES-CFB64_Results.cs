using System.Collections.Generic;

namespace LCAVPCore.AlgorithmResults.TDES
{
	public class TDES_CFB64_Results : AlgorithmResultsBase
	{
		public List<PassFailResult> Encrypt { get; set; }
		public List<PassFailResult> Decrypt { get; set; }
	}
}