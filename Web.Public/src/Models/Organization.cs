using System.Collections.Generic;
using System.Text.Json.Serialization;
using Web.Public.JsonObjects;

namespace Web.Public.Models
{
	public class Organization : IJsonObject
	{
		[JsonIgnore]
		public long ID { get; set; }

		[JsonPropertyName("url")]
		public string URL => $"/acvp/v1/vendors/{ID}";

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("website")]
		public string Website { get; set; }

		[JsonPropertyName("emails")]
		public List<string> Emails { get; set; }

		[JsonPropertyName("phoneNumbers")]
		public List<PhoneNumber> PhoneNumbers { get; set; }

		[JsonPropertyName("contactsUrl")]
		public string ContactsURL => $"/acvp/v1/vendors/{ID}/contacts";

		[JsonPropertyName("addresses")]
		public List<Address> Addresses { get; set; }

		public List<string> ValidateObject()
		{
			return new List<string>();
		}
	}
}
