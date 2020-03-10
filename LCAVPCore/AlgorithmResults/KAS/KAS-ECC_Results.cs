using System.Collections.Generic;

namespace LCAVPCore.AlgorithmResults.KAS
{
	public class KAS_ECC_Results : AlgorithmResultsBase
	{
		public List<PassFailResult> EphemeralUnified_Functionality { get; set; } = new List<PassFailResult>();
		public List<PassFailResult> EphemeralUnified_Validity { get; set; } = new List<PassFailResult>();
		public List<PassFailResult> FullMQV_Functionality { get; set; } = new List<PassFailResult>();
		public List<PassFailResult> FullMQV_Validity { get; set; } = new List<PassFailResult>();
		public List<PassFailResult> FullUnified_Functionality { get; set; } = new List<PassFailResult>();
		public List<PassFailResult> FullUnified_Validity { get; set; } = new List<PassFailResult>();
		public List<PassFailResult> OnePassDH_Functionality { get; set; } = new List<PassFailResult>();
		public List<PassFailResult> OnePassDH_Validity { get; set; } = new List<PassFailResult>();
		public List<PassFailResult> OnePassMQV_Functionality { get; set; } = new List<PassFailResult>();
		public List<PassFailResult> OnePassMQV_Validity { get; set; } = new List<PassFailResult>();
		public List<PassFailResult> OnePassUnified_Functionality { get; set; } = new List<PassFailResult>();
		public List<PassFailResult> OnePassUnified_Validity { get; set; } = new List<PassFailResult>();
		public List<PassFailResult> StaticUnified_Functionality { get; set; } = new List<PassFailResult>();
		public List<PassFailResult> StaticUnified_Validity { get; set; } = new List<PassFailResult>();
	}
}