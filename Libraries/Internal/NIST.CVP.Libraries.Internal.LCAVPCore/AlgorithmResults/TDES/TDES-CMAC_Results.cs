namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults.TDES
{
	public class TDES_CMAC_Results : AlgorithmResultsBase
	{
		public bool KeySize2GeneratePassed { get; set; }
		public bool KeySize2VerifyPassed { get; set; }
		public bool KeySize3GeneratePassed { get; set; }
		public bool KeySize3VerifyPassed { get; set; }
	}
}