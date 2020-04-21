using System.Collections.Generic;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults.Component
{
	public class Component_KAS_ECC_Results : AlgorithmResultsBase
	{
		public List<PassFailResult> EphemeralUnified_Functionality { get; set; }
		public List<PassFailResult> EphemeralUnified_Validity { get; set; }
		public List<PassFailResult> FullMQV_Functionality { get; set; }
		public List<PassFailResult> FullMQV_Validity { get; set; }
		public List<PassFailResult> FullUnified_Functionality { get; set; }
		public List<PassFailResult> FullUnified_Validity { get; set; }
		public List<PassFailResult> OnePassDH_Functionality { get; set; }
		public List<PassFailResult> OnePassDH_Validity { get; set; }
		public List<PassFailResult> OnePassMQV_Functionality { get; set; }
		public List<PassFailResult> OnePassMQV_Validity { get; set; }
		public List<PassFailResult> OnePassUnified_Functionality { get; set; }
		public List<PassFailResult> OnePassUnified_Validity { get; set; }
		public List<PassFailResult> StaticUnified_Functionality { get; set; }
		public List<PassFailResult> StaticUnified_Validity { get; set; }
	}
}