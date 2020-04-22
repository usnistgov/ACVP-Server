namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults.SHA_3
{
	public class SHAKE_128_Results : AlgorithmResultsBase
	{
		public bool Pass { get; set; }
		public bool InputByteOrientedOnly { get; set; }
		public bool OutputByteOrientedOnly { get; set; }
		public bool DoesNotSupportNullMessage { get; set; }
	}
}
