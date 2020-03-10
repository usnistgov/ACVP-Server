namespace LCAVPCore.AlgorithmResults.AES
{
	public class AES_XPN_Results : AlgorithmResultsBase
	{
		public bool Encrypt128Passed { get; set; }
		public bool Decrypt128Passed { get; set; }
		public bool Encrypt256Passed { get; set; }
		public bool Decrypt256Passed { get; set; }
	}
}