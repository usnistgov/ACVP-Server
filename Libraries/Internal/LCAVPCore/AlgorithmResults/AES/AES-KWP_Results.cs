namespace LCAVPCore.AlgorithmResults.AES
{
	public class AES_KWP_Results : AlgorithmResultsBase
	{
		public bool Encrypt128Passed { get; set; }
		public bool Decrypt128Passed { get; set; }
		public bool Encrypt192Passed { get; set; }
		public bool Decrypt192Passed { get; set; }
		public bool Encrypt256Passed { get; set; }
		public bool Decrypt256Passed { get; set; }
		public bool Encrypt128InversePassed { get; set; }
		public bool Decrypt128InversePassed { get; set; }
		public bool Encrypt192InversePassed { get; set; }
		public bool Decrypt192InversePassed { get; set; }
		public bool Encrypt256InversePassed { get; set; }
		public bool Decrypt256InversePassed { get; set; }
	}
}