namespace LCAVPCore.AlgorithmResults.Component
{
	public class IKEv1_Results : AlgorithmResultsBase
	{
		public bool DigitalSignatureAuthenticationPassed { get; set; }
		public bool PublicKeyEncryptionAuthenticationPassed { get; set; }
		public bool PresharedKeyAuthenticationPassed { get; set; }
	}
}
