using System.Collections.Generic;

namespace LCAVPCore.AlgorithmResults.AES
{
	public class AES_ECB_Results : AlgorithmResultsBase
	{
		public List<PassFailResult> Encrypt128 { get; set; }
		public List<PassFailResult> Decrypt128 { get; set; }
		public List<PassFailResult> Encrypt192 { get; set; }
		public List<PassFailResult> Decrypt192 { get; set; }
		public List<PassFailResult> Encrypt256 { get; set; }
		public List<PassFailResult> Decrypt256 { get; set; }
	}
}