using Newtonsoft.Json;
using System.Collections.Generic;

namespace LCAVPCore.Registration
{
	public class Dependency : IDependency
	{
		[JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
		public string Url { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		//None of the following will be used in things produced by LCAVP, but they could come in the response from the web service Get

		[JsonProperty("swid", NullValueHandling = NullValueHandling.Ignore)]
		public string SWID { get; set; }

		[JsonProperty("cpe", NullValueHandling = NullValueHandling.Ignore)]
		public string CPE { get; set; }

		[JsonProperty("manufacturer", NullValueHandling = NullValueHandling.Ignore)]
		public string Manufacturer { get; set; }

		[JsonProperty("family", NullValueHandling = NullValueHandling.Ignore)]
		public string Family { get; set; }

		[JsonProperty("series", NullValueHandling = NullValueHandling.Ignore)]
		public string Series { get; set; }

		[JsonProperty("features", NullValueHandling = NullValueHandling.Ignore)]
		public List<string> Features { get; set; }

		[JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
		public string Description { get => Name; }
	}
}
