﻿using System.Collections.Generic;

namespace LCAVPCore.AlgorithmResults.AES
{
	public class AES_CTR_Results : AlgorithmResultsBase
	{
		public bool Passed128 { get; set; }
		public bool Passed192 { get; set; }
		public bool Passed256 { get; set; }
	}
}