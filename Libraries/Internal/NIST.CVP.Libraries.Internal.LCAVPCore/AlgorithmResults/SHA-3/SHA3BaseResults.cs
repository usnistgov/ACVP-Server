namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults.SHA_3
{
	public class SHA3BaseResults : AlgorithmResultsBase
	{
		public bool Pass { get; set; }
		public bool ByteOrientedOnly { get; set; }
		public bool DoesNotSupportNullMessage { get; set; }
	}
}