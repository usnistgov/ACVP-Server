using System.Collections.Generic;

namespace LCAVPCore.AlgorithmResults.TDES
{
	public class TDES_CBCI_Results : AlgorithmResultsBase
	{
		public List<PassFailResult> Encrypt { get; set; }
		public List<PassFailResult> Decrypt { get; set; }
	}
}