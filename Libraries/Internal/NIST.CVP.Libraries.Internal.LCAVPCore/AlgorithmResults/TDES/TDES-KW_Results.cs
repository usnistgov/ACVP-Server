namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults.TDES
{
	public class TDES_KW_Results : AlgorithmResultsBase
	{
		public bool EncryptPassed { get; set; }
		public bool DecryptPassed { get; set; }
		public bool EncryptInversePassed { get; set; }
		public bool DecryptInversePassed { get; set; }
	}
}