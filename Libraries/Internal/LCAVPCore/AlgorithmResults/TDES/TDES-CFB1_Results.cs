using System.Collections.Generic;

namespace LCAVPCore.AlgorithmResults.TDES
{
	public class TDES_CFB1_Results : AlgorithmResultsBase
	{
		public List<PassFailResult> Encrypt { get; set; }
		public List<PassFailResult> Decrypt { get; set; }
	}
}