using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using ACVPCore.Models.Parameters;

namespace ACVPWorkflow.Models
{
	public class OrganizationUpdatePayload : BasePayload, IWorkflowItemPayload
	{
		private string _name;
		private string _website;
		private string _parentOrganizationURL;
		private List<string> _emailAddresses;
		private string _voiceNumber;
		private string _faxNumber;
		private List<Address> _addresses;

		[JsonPropertyName("id")]
		public long ID { get; set; }

		[JsonPropertyName("url")]
		public string URL { get => $"/admin/organizations/{ID}"; }

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

		[JsonPropertyName("link")]
		public string Website
		{
			get => _website;
			set
			{
				_website = value;
				WebsiteUpdated = true;
			}
		}

		[JsonPropertyName("parentUrl")]
		public string ParentOrganizationURL
		{
			get => _parentOrganizationURL;
			set
			{
				_parentOrganizationURL = value;
				ParentOrganizationURLUpdated = true;
			}
		}

		[JsonPropertyName("emails")]
		public List<string> EmailAddresses
		{
			get => _emailAddresses;
			set
			{
				_emailAddresses = value;
				EmailAddressesUpdated = true;
			}
		}

		[JsonPropertyName("voiceNumber")]
		public string VoiceNumber
		{
			get => _voiceNumber;
			set
			{
				_voiceNumber = value;
				VoiceNumberUpdated = true;
			}
		}

		[JsonPropertyName("faxNumber")]
		public string FaxNumber
		{
			get => _faxNumber;
			set
			{
				_faxNumber = value;
				FaxNumberUpdated = true;
			}
		}

		[JsonPropertyName("addresses")]
		public List<Address> Addresses {
			get => _addresses;
			set
			{
				_addresses = value;
				AddressesUpdated = true;
			}
		}

		public bool NameUpdated { get; private set; } = false;
		public bool WebsiteUpdated { get; private set; } = false;
		public bool ParentOrganizationURLUpdated { get; private set; } = false;
		public bool VoiceNumberUpdated { get; private set; } = false;
		public bool FaxNumberUpdated { get; private set; } = false;
		public bool EmailAddressesUpdated { get; private set; } = false;
		public bool AddressesUpdated { get; private set; } = false;


		public OrganizationUpdateParameters ToOrganizationUpdateParameters()
		{
			//Build a OrganizationUpdateParameters object. The nested addresses are a bit goofy since they can be new or updates, and different handling is required
			List<object> AddressObjects = new List<object>();

			if (Addresses != null)
			{
				for (int i = 0; i < Addresses.Count; i++)
				{
					var addressParameters = Addresses[i];

					if (string.IsNullOrEmpty(addressParameters.URL))
					{
						//This will be a new address
						AddressObjects.Add(new AddressCreateParameters
						{
							OrganizationID = ID,
							OrderIndex = i,
							Street1 = addressParameters.Street1,
							Street2 = addressParameters.Street2,
							Street3 = addressParameters.Street3,
							Locality = addressParameters.Locality,
							Region = addressParameters.Region,
							PostalCode = addressParameters.PostalCode,
							Country = addressParameters.Country
						});
					}
					else
					{
						//This is an address update
						AddressObjects.Add(new AddressUpdateParameters
						{
							ID = long.Parse(addressParameters.URL.Split("/")[^1]),
							OrderIndex = i,
							Street1 = addressParameters.Street1,
							Street2 = addressParameters.Street2,
							Street3 = addressParameters.Street3,
							Locality = addressParameters.Locality,
							Region = addressParameters.Region,
							PostalCode = addressParameters.PostalCode,
							Country = addressParameters.Country,
							Street1Updated = addressParameters.Street1Updated,
							Street2Updated = addressParameters.Street2Updated,
							Street3Updated = addressParameters.Street3Updated,
							LocalityUpdated = addressParameters.LocalityUpdated,
							RegionUpdated = addressParameters.RegionUpdated,
							PostalCodeUpdated = addressParameters.PostalCodeUpdated,
							CountryUpdated = addressParameters.CountryUpdated
							//Not passing org id because can't update that
						});
					}
				}
			}

			return new OrganizationUpdateParameters
			{
				ID = ID,
				Name = Name,
				Website = Website,
				VoiceNumber = VoiceNumber,
				FaxNumber = FaxNumber,
				ParentOrganizationID = ParseNullableIDFromURL(ParentOrganizationURL),
				EmailAddresses = EmailAddresses,
				Addresses = AddressObjects,
				NameUpdated = NameUpdated,
				WebsiteUpdated = WebsiteUpdated,
				ParentOrganizationIDUpdated = ParentOrganizationURLUpdated,
				VoiceNumberUpdated = VoiceNumberUpdated,
				FaxNumberUpdated = FaxNumberUpdated,
				EmailAddressesUpdated = EmailAddressesUpdated,
				AddressesUpdated = AddressesUpdated
			};
		}





		public class PhoneNumber
		{
			[JsonPropertyName("number")]
			public string Number { get; set; }

			[JsonPropertyName("type")]
			public string Type { get; set; }
		}

		public class Address
		{
			private string _street1;
			private string _street2;
			private string _street3;
			private string _locality;
			private string _region;
			private string _postalCode;
			private string _country;

			[JsonPropertyName("url")]
			public string URL { get; set; }

			[JsonPropertyName("street1")]
			public string Street1
			{
				get => _street1;
				set
				{
					_street1 = value;
					Street1Updated = true;
				}
			}

			[JsonPropertyName("street2")]
			public string Street2
			{
				get => _street2;
				set
				{
					_street2 = value;
					Street2Updated = true;
				}
			}

			[JsonPropertyName("street3")]
			public string Street3
			{
				get => _street3;
				set
				{
					_street3 = value;
					Street3Updated = true;
				}
			}

			[JsonPropertyName("locality")]
			public string Locality
			{
				get => _locality;
				set
				{
					_locality = value;
					LocalityUpdated = true;
				}
			}

			[JsonPropertyName("region")]
			public string Region
			{
				get => _region;
				set
				{
					_region = value;
					RegionUpdated = true;
				}
			}

			[JsonPropertyName("country")]
			public string Country
			{
				get => _country;
				set
				{
					_country = value;
					CountryUpdated = true;
				}
			}

			[JsonPropertyName("postalCode")]
			public string PostalCode
			{
				get => _postalCode;
				set
				{
					_postalCode = value;
					PostalCodeUpdated = true;
				}
			}


			public bool Street1Updated { get; private set; } = false;
			public bool Street2Updated { get; private set; } = false;
			public bool Street3Updated { get; private set; } = false;
			public bool LocalityUpdated { get; private set; } = false;
			public bool RegionUpdated { get; private set; } = false;
			public bool PostalCodeUpdated { get; private set; } = false;
			public bool CountryUpdated { get; private set; } = false;
		}
	}
}
