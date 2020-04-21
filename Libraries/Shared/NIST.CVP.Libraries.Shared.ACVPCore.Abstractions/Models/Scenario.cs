using System.Collections.Generic;

namespace NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models
{
	public class Scenario
	{
		public long? ID { get; set; }
		public long? ValidationID { get; set; }
		public List<long> ScenarioAlgorithms { get; set; }
		public List<long> OperatingEnvironments { get; set; }
	}
}
