using System.Collections.Generic;
using Newtonsoft.Json;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Registration
{
	public class NewRegistrationContainer
	{
		[JsonProperty("product")]
		public Module Module { get; set; }

		[JsonProperty("scenarios")]
		public List<Scenario> Scenarios { get; set; } = new List<Scenario>();	
	}
}
