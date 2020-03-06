using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using ACVPCore.Models.Parameters;

namespace ACVPWorkflow.Models
{
	public class OrganizationCreatePayload : BasePayload, IWorkflowItemPayload
	{
		[JsonPropertyName("id")]
		public long ID { get => -1; }

		[JsonPropertyName("url")]
		public string URL { get => "/admin/organizations/-1"; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("link")]
		public string Website { get; set; }

		[JsonPropertyName("parentUrl")]
		public string ParentURL { get; set; }

		[JsonPropertyName("emails")]
		public List<string> EmailAddresses { get; set; }

		[JsonPropertyName("voiceNumber")]
		public string VoiceNumber { get; set; }

		[JsonPropertyName("faxNumber")]
		public string FaxNumber { get; set; }

		[JsonPropertyName("addresses")]
		public List<Address> Addresses { get; set; }


		public OrganizationCreateParameters ToOrganizationCreateParameters() => new OrganizationCreateParameters
		{
			Name = Name,
			Website = Website,
			VoiceNumber = VoiceNumber,
			FaxNumber = FaxNumber,
			ParentOrganizationID = ParseNullableIDFromURL(ParentURL),
			EmailAddresses = EmailAddresses,
			Addresses = Addresses?.Select(x => new AddressCreateParameters
			{
				Street1 = x.Street1,
				Street2 = x.Street2,
				Street3 = x.Street3,
				Locality = x.Locality,
				Region = x.Region,
				PostalCode = x.PostalCode,
				Country = x.Country
			}).ToList()
		};

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
