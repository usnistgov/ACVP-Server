using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ACVPWorkflow.Models
{
	public class PersonCreatePayload
	{
		[JsonPropertyName("id")]
		public long ID { get => -1; }

		[JsonPropertyName("url")]
		public string URL { get => "/admin/persons/-1"; }

		[JsonPropertyName("fullName")]
		public string Name { get; set; }

		[JsonPropertyName("vendorUrl")]
		public string VendorURL { get; set; }

		[JsonPropertyName("emails")]
		public List<string> EmailAddresses { get; set; }

		[JsonPropertyName("phoneNumbers")]
		public List<PhoneNumber> PhoneNumbers { get; set; }

		public class PhoneNumber
		{
			[JsonPropertyName("number")]
			public string Number { get; set; }

			[JsonPropertyName("type")]
			public string Type { get; set; }
		}
	}
}
