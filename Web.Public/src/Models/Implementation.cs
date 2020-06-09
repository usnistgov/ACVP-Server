using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;

namespace Web.Public.Models
{
	public class Implementation
	{
		[JsonIgnore]
		public long ID { get; set; }

		[JsonPropertyName("url")]
		public string URL => $"/acvp/v1/modules/{ID}";

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("version")]
		public string Version { get; set; }

		[JsonIgnore]
		public ImplementationType Type { get; set; }

		[JsonPropertyName("type")]
		public string TypeString
		{
			get => Type switch
			{
				ImplementationType.Software => "software",
				ImplementationType.Hardware => "hardware",
				ImplementationType.Firmware => "firmware",
				_ => "unknown"
			};
			set => Type = value.ToLower() switch
			{
				"software" => ImplementationType.Software,
				"hardware" => ImplementationType.Hardware,
				"firmware" => ImplementationType.Firmware,
				_ => ImplementationType.Unknown
			};
		}

		[JsonPropertyName("website")]
		public string Website { get; set; }

		[JsonIgnore]
		public long OrganizationID { get; set; }

		[JsonPropertyName("vendorUrl")]
		public string VendorURL => $"/acvp/v1/vendors/{OrganizationID}";

		[JsonIgnore]
		public long? AddressID { get; set; }

		[JsonPropertyName("addressUrls")]
		public string AddressURL => $"/acvp/v1/vendors/{OrganizationID}/addresses/{AddressID}";

		[JsonIgnore]
		public List<long> ContactIDs { get; set; }

		[JsonPropertyName("contactUrls")]
		public List<string> ContactUrls => ContactIDs?.Select(x => $"/acvp/v1/persons/{x}").ToList();

		[JsonPropertyName("description")]
		public string Description { get; set; }

		public List<string> ValidateObject()
		{
			var errors = new List<string>();

			//TODO - something

			return errors;
		}
	}
}
