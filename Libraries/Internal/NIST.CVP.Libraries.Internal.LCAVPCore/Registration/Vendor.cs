using System.Collections.Generic;
using Newtonsoft.Json;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Registration
{
	public class Vendor
	{
		[JsonIgnore]
		public int ID { get; set; }

		[JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
		public string Url { get { return ID == 0 ? null : $"/admin/validations/organizations/{ID}"; } }

		[JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
		public string Name { get; set; }

		[JsonProperty("link", NullValueHandling = NullValueHandling.Ignore)]
		public string Website { get; set; }

		[JsonProperty("emails", NullValueHandling = NullValueHandling.Ignore)]
		public List<string> Emails { get; set; }

		[JsonProperty("addresses", NullValueHandling = NullValueHandling.Ignore)]
		public List<Address> Address { get; set; }

		//Added as a workaround for disjoint between the old logic and the new logic due to nested stuff making it hard to tell what actually changed
		[JsonIgnore]
		public bool NameUpdated { get; set; } = false;

		[JsonIgnore]
		public bool WebsiteUpdated { get; set; } = false;

		[JsonIgnore]
		public bool AddressUpdated { get; set; } = false;
	}
}
