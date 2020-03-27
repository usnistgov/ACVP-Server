using System.Collections.Generic;
using Newtonsoft.Json;

namespace LCAVPCore.Registration
{
	public class OperationalEnvironment
	{
		[JsonIgnore]
		public int ID { get; set; }

		[JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
		public string Url { get { return ID == 0 ? null : $"/admin/validations/oes/{ID}"; } }

		[JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
		public string Name { get; set; }

		[JsonProperty("dependencies", NullValueHandling = NullValueHandling.Ignore)]
		public List<IDependency> Dependencies { get; set; }
	}
}
