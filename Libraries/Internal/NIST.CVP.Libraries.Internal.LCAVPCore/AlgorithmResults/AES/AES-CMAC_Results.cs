namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults.AES
{
	public class AES_CMAC_Results : AlgorithmResultsBase
	{
		public bool Generate128Passed { get; set; }
		public bool Verify128Passed { get; set; }
		public bool Generate192Passed { get; set; }
		public bool Verify192Passed { get; set; }
		public bool Generate256Passed { get; set; }
		public bool Verify256Passed { get; set; }
	}
}