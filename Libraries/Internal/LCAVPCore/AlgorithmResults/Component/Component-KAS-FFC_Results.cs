using System.Collections.Generic;

namespace LCAVPCore.AlgorithmResults.Component
{
	public class Component_KAS_FFC_Results : AlgorithmResultsBase
	{
		public List<PassFailResult> DHEphem_Functionality { get; set; }
		public List<PassFailResult> DHEphem_Validity { get; set; }
		public List<PassFailResult> DHHybrid1_Functionality { get; set; }
		public List<PassFailResult> DHHybrid1_Validity { get; set; }
		public List<PassFailResult> DHHybrid1Flow_Functionality { get; set; }
		public List<PassFailResult> DHHybrid1Flow_Validity { get; set; }
		public List<PassFailResult> DHOneFlow_Functionality { get; set; }
		public List<PassFailResult> DHOneFlow_Validity { get; set; }
		public List<PassFailResult> DHStatic_Functionality { get; set; }
		public List<PassFailResult> DHStatic_Validity { get; set; }
		public List<PassFailResult> MQV1_Functionality { get; set; }
		public List<PassFailResult> MQV1_Validity { get; set; }
		public List<PassFailResult> MQV2_Functionality { get; set; }
		public List<PassFailResult> MQV2_Validity { get; set; }



	}
}