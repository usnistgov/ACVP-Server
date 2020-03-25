namespace LCAVPCore.AlgorithmResults.SHS
{
	public class SHSBaseResults : AlgorithmResultsBase
	{
		public bool Pass { get; set; }
		public bool ByteOrientedOnly { get; set; }
		public bool DoesNotSupportNullMessage { get; set; }
	}
}