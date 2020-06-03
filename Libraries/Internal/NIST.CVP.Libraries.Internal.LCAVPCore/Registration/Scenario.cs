using System.Collections.Generic;
using Newtonsoft.Json;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Registration
{
	public class Scenario
	{
		[JsonProperty("operatingEnvironments")]
		public OperationalEnvironment OE { get; set; }

		[JsonProperty("algorithmCapabilities")]
		public List<AlgorithmCapabilities> Algorithms { get; set; } = new List<AlgorithmCapabilities>();
	}
}
