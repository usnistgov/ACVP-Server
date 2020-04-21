using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using NIST.CVP.Libraries.Internal.ACVPCore;
using NIST.CVP.Libraries.Internal.ACVPCore.Models.Parameters;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.Models
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

		[JsonPropertyName("link")]      //website in the public json...
		public string Website { get; set; }

		//This matches the public API - return to it in the future
		//[JsonPropertyName("vendorUrl")]
		//public string VendorURL { get; set; }

		[JsonPropertyName("addressUrl")]
		public string AddressURL { get; set; }

		//This matches the public API - return to it in the future
		//[JsonPropertyName("contactUrls")]
		//public List<string> ContactURLs { get; set; }

		[JsonPropertyName("vendor")]
		public Vendor VendorObjectThatNeedsToGoAway { get; set; }

		[JsonPropertyName("contacts")]
		public List<Contact> ContactsObjectThatNeedsToGoAway { get; set; }

		public ImplementationCreateParameters ToImplementationCreateParameters() => new ImplementationCreateParameters
		{
			Name = Name,
			Description = Description,
			Type = ParseImplementationType(Type),
			Version = Version,
			Website = Website,
			//OrganizationID = ParseIDFromURL(VendorURL),			//return to this in the future
			OrganizationID = VendorObjectThatNeedsToGoAway.ID,
			AddressID = ParseNullableIDFromURL(AddressURL),
			//ContactIDs = ContactURLs?.Select(x => ParseIDFromURL(x)).ToList(),		//return to this in the future
			ContactIDs = ContactsObjectThatNeedsToGoAway?.OrderBy(x => x.OrderIndex).Select(x => x.Person.ID).ToList(),
			IsITAR = false      //TODO - Do something for ITARs. For now, assuming nothing is ITAR
		};

		private ImplementationType ParseImplementationType(string type) => type.ToLower() switch
		{
			"software" => ImplementationType.Software,
			"hardware" => ImplementationType.Hardware,
			"firmware" => ImplementationType.Firmware,
			_ => ImplementationType.Unknown
		};


		//These classes exist only to handle the senseless things that are done to screw up the data sent by the user before it gets put in the message. They can be removed when Public is rewritten to use reasonable messages.
		public class Vendor
		{
			[JsonPropertyName("id")]
			public long ID { get; set; }
		}

		public class Contact
		{
			[JsonPropertyName("orderIndex")]
			public long OrderIndex { get; set; }

			[JsonPropertyName("person")]
			public Person Person { get; set; }
		}

		public class Person
		{
			[JsonPropertyName("id")]
			public long ID { get; set; }
		}
	}
}
