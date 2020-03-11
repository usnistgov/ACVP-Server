using System.Collections.Generic;

namespace LCAVPCore.AlgorithmResults.RSA
{
	public class RSAKeyGen_Results : AlgorithmResultsBase
	{
		public List<PassFailResult> Results { get; set; } = new List<PassFailResult>();
	}
}