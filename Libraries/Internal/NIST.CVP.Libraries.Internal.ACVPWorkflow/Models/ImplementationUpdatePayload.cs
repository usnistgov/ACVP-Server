using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using NIST.CVP.Libraries.Internal.ACVPCore;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.Models
{
	public class ImplementationUpdatePayload : BasePayload, IWorkflowItemPayload
	{
		private string _name;
		private string _description;
		private string _type;
		private string _version;
		private string _website;
		//private string _vendorURL;
		private string _addressURL;
		//private List<string> _contactUrls;

		[JsonPropertyName("id")]
		public long ID { get; set; }

		[JsonPropertyName("url")]
		public string URL => $"/admin/modules/{ID}";

		[JsonPropertyName("name")]
		public string Name
		{
			get => _name;
			set
			{
				_name = value;
				NameUpdated = true;
			}
		}

		[JsonPropertyName("description")]
		public string Description
		{
			get => _description;
			set
			{
				_description = value;
				DescriptionUpdated = true;
			}
		}

		[JsonPropertyName("type")]
		public string Type
		{
			get => _type;
			set
			{
				_type = value;
				TypeUpdated = true;
			}
		}

		[JsonPropertyName("version")]
		public string Version
		{
			get => _version;
			set
			{
				_version = value;
				VersionUpdated = true;
			}
		}

		[JsonPropertyName("website")]
		public string Website
		{
			get => _website;
			set
			{
				_website = value;
				WebsiteUpdated = true;
			}
		}

		//return to this in the future when Public is rewritten
		//[JsonPropertyName("vendorUrl")]
		//public string VendorURL {
		//	get => _vendorURL;
		//	set
		//	{
		//		_vendorURL = value;
		//		VendorURLUpdated = true;
		//	}
		//}

		[JsonPropertyName("addressUrl")]
		public string AddressURL
		{
			get => _addressURL;
			set
			{
				_addressURL = value;
				AddressURLUpdated = true;
			}
		}

		//return to this in the future when Public is rewritten
		//[JsonPropertyName("contactUrls")]
		//public List<string> ContactURLs {
		//	get => _contactUrls;
		//	set
		//	{
		//		_contactUrls = value;
		//		ContactURLsUpdated = true;
		//	}
		//}

		public bool NameUpdated { get; set; } = false;
		public bool DescriptionUpdated { get; set; } = false;
		public bool TypeUpdated { get; set; } = false;
		public bool VersionUpdated { get; set; } = false;
		public bool WebsiteUpdated { get; set; } = false;
		public bool VendorURLUpdated { get; set; } = false;
		public bool AddressURLUpdated { get; set; } = false;
		public bool ContactURLsUpdated { get; set; } = false;

		//Everything below this is here only to handle the goofy messages. It should go away once Public is rewritten
		private long VendorID { get; set; }
		private List<long> ContactIDs => _contacts?.OrderBy(x => x.OrderIndex).Select(x => x.Person.ID).ToList();		// { get; set; }

		private Vendor _vendor;
		private List<Contact> _contacts;

		[JsonPropertyName("vendor")]
		public Vendor VendorObjectThatNeedsToGoAway
		{
			get => _vendor;
			set
			{
				_vendor = value;
				VendorID = value.ID;
				VendorURLUpdated = true;
			}
		}

		[JsonPropertyName("contacts")]
		public List<Contact> ContactsObjectThatNeedsToGoAway
		{
			get => _contacts;
			set
			{
				_contacts = value;
				//ContactIDs = _contacts?.OrderBy(x => x.OrderIndex).Select(x => x.Person.ID).ToList();
				ContactURLsUpdated = true;
			}
		}





		public ImplementationUpdateParameters ToImplementationUpdateParameters() => new ImplementationUpdateParameters
		{
			ID = ID,
			Name = Name,
			Description = Description,
			Type = ParseImplementationType(Type),
			Version = Version,
			Website = Website,
			//OrganizationID = ParseNullableIDFromURL(VendorURL),		//return to this in the future
			OrganizationID = VendorID,
			AddressID = ParseNullableIDFromURL(AddressURL),
			//ContactIDs = ContactURLs?.Select(x => ParseIDFromURL(x))?.ToList(),		//return to this in the future
			ContactIDs = ContactIDs,
			NameUpdated = NameUpdated,
			DescriptionUpdated = DescriptionUpdated,
			TypeUpdated = TypeUpdated,
			VersionUpdated = VersionUpdated,
			WebsiteUpdated = WebsiteUpdated,
			OrganizationIDUpdated = VendorURLUpdated,
			AddressIDUpdated = AddressURLUpdated,
			ContactIDsUpdated = ContactURLsUpdated
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
