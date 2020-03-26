using System.Text.Json.Serialization;

namespace Web.Public.Models
{
	public class Address
	{
		[JsonIgnore]
		public long ID { get; set; }

		[JsonPropertyName("url")]
		public string URL => $"/acvp/v1/vendors/{OrganizationID}/addresses/{ID}";

		[JsonIgnore]
		public long OrganizationID { get; set; }

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
