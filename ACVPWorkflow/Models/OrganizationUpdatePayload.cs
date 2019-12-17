using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ACVPWorkflow.Models
{
	public class OrganizationUpdatePayload
	{
		private string _name;
		private string _website;
		private string _parentOrganizationURL;
		private List<string> _emailAddresses;
		private List<PhoneNumber> _phoneNumbers;
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

		[JsonPropertyName("phoneNumbers")]
		public List<PhoneNumber> PhoneNumbers
		{
			get => _phoneNumbers;
			set
			{
				_phoneNumbers = value;
				PhoneNumbersUpdated = true;
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
		public bool PhoneNumbersUpdated { get; private set; } = false;
		public bool EmailAddressesUpdated { get; private set; } = false;
		public bool AddressesUpdated { get; private set; } = false;

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
