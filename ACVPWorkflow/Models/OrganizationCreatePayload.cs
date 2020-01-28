using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ACVPWorkflow.Models
{
	public class OrganizationCreatePayload
	{
		[JsonPropertyName("id")]
		public long ID { get => -1; }

		[JsonPropertyName("url")]
		public string URL { get => "/admin/organizations/-1"; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("website")]
		public string Website { get; set; }

		[JsonPropertyName("parentUrl")]
		public string ParentURL { get; set; }

		[JsonPropertyName("emails")]
		public List<string> EmailAddresses { get; set; }

		[JsonPropertyName("phoneNumbers")]
		public List<PhoneNumber> PhoneNumbers { get; set; }

		[JsonPropertyName("addresses")]
		public List<Address> Addresses { get; set; }

		public class PhoneNumber
		{
			[JsonPropertyName("number")]
			public string Number { get; set; }

			[JsonPropertyName("type")]
			public string Type { get; set; }
		}

		public class Address
		{
			[JsonPropertyName("street1")]
			public string Street1 { get; set; }

			[JsonPropertyName("street2")]
			public string Street2 { get; set; }

			[JsonPropertyName("street3")]
			public string Street3 { get; set; }

			[JsonPropertyName("locality")]
			public string Locality { get; set; }

			[JsonPropertyName("region")]
			public string Region { get; set; }

			[JsonPropertyName("country")]
			public string Country { get; set; }

			[JsonPropertyName("postalCode")]
			public string PostalCode { get; set; }
		}
	}
}
