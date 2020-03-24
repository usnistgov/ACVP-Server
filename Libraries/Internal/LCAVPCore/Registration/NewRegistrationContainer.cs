using System.Collections.Generic;
using Newtonsoft.Json;

namespace LCAVPCore.Registration
{
	public class NewRegistrationContainer
	{
		[JsonProperty("product")]
		public Module Module { get; set; }

		[JsonProperty("scenarios")]
		public List<Scenario> Scenarios { get; set; } = new List<Scenario>();	
	}
}
