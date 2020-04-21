using System.Collections.Generic;
using Newtonsoft.Json;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Registration
{
	public class UpdateRegistrationContainer
	{
		[JsonIgnore]
		public int ValidationID { get; set; }

		[JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
		public string ValidationUrl { get => ValidationID == 0 ? null : $"/admin/validations/{ValidationID}"; }

		[JsonIgnore]
		public int ModuleID { get; set; }

		[JsonProperty("productUrl", NullValueHandling = NullValueHandling.Ignore)]
		public string ModuleUrl { get => ModuleID == 0 ? null : $"/admin/validations/products/{ModuleID}"; }

		[JsonProperty("scenarios")]
		public List<Scenario> Scenarios { get; set; } = new List<Scenario>();	
	}
}
