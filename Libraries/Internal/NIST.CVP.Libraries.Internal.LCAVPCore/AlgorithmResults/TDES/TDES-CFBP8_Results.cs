using System.Collections.Generic;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults.TDES
{
	public class TDES_CFBP8_Results : AlgorithmResultsBase
	{
		public List<PassFailResult> Encrypt { get; set; }
		public List<PassFailResult> Decrypt { get; set; }
	}
}