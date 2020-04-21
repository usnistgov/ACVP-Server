using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Models
{
	public class Scenario
	{
		public long? ID { get; set; }
		public long? ValidationID { get; set; }
		public List<long> ScenarioAlgorithms { get; set; }
		public List<long> OperatingEnvironments { get; set; }
	}
}
