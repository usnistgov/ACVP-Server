using Newtonsoft.Json;
using System.Collections.Generic;

namespace LCAVPCore.Registration
{
	public class Module
	{
		[JsonIgnore]
		public int ID { get; set; }

		[JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
		public string Url { get { return ID == 0 ? null : $"/admin/validations/products/{ID}"; } }

		[JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
		public string Name { get; set; }

		[JsonProperty("version", NullValueHandling = NullValueHandling.Ignore)]
		public string Version { get; set; }

		[JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
		public string Type { get; set; }

		[JsonProperty("vendor", NullValueHandling = NullValueHandling.Ignore)]
		public Vendor Vendor { get; set; }

		[JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
		public string Description { get; set; }

		[JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
		public Address Address { get; set; }

		[JsonProperty("itar")]
		public bool ITAR { get => false; }

		[JsonProperty("persons")]
		public List<Contact> Contacts { get; set; } = new List<Contact>();

		public bool ShouldSerializeContacts()
		{
			return Contacts.Count > 0;
		}


		//Added as a workaround for disjoint between the old logic and the new logic due to nested stuff making it hard to tell what actually changed
		[JsonIgnore]
		public bool NameUpdated { get; set; } = false;

		[JsonIgnore]
		public bool VersionUpdated { get; set; } = false;

		[JsonIgnore]
		public bool TypeUpdated { get; set; } = false;

		[JsonIgnore]
		public bool DescriptionUpdated { get; set; } = false;

		[JsonIgnore]
		public bool Contact1Added { get; set; } = false;

		[JsonIgnore]
		public bool Contact2Added { get; set; } = false;
	}
}
