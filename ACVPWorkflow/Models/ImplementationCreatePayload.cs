using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using ACVPCore;
using ACVPCore.Models.Parameters;

namespace ACVPWorkflow.Models
{
	public class ImplementationCreatePayload : BasePayload, IWorkflowItemPayload
	{
		[JsonPropertyName("id")]
		public long ID { get => -1; }

		[JsonPropertyName("url")]
		public string URL { get => "/admin/modules/-1"; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("description")]
		public string Description { get; set; }

		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("version")]
		public string Version { get; set; }

		[JsonPropertyName("website")]
		public string Website { get; set; }

		[JsonPropertyName("vendorUrl")]
		public string VendorURL { get; set; }

		[JsonPropertyName("addressUrl")]
		public string AddressURL { get; set; }

		[JsonPropertyName("contactUrls")]
		public List<string> ContactURLs { get; set; }


		public ImplementationCreateParameters ToImplementationCreateParameters() => new ImplementationCreateParameters
		{
			Name = Name,
			Description = Description,
			Type = ParseImplementationType(Type),
			Version = Version,
			Website = Website,
			OrganizationID = ParseIDFromURL(VendorURL),
			AddressID = ParseIDFromURL(AddressURL),
			ContactIDs = ContactURLs.Select(x => ParseIDFromURL(x)).ToList(),
			IsITAR = false      //TODO - Do something for ITARs. For now, assuming nothing is ITAR
		};

		private ImplementationType ParseImplementationType(string type) => type.ToLower() switch
		{
			"software" => ImplementationType.Software,
			"hardware" => ImplementationType.Hardware,
			"firmware" => ImplementationType.Firmware,
			_ => ImplementationType.Unknown
		};
	}
}
