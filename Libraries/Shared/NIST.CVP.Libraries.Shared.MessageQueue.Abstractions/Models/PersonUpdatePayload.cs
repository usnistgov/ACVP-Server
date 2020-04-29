using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;

namespace NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models
{
	public class PersonUpdatePayload : BasePayload, IWorkflowItemPayload
	{
		private string _name;
		private string _organizationURL;
		private List<string> _emailAddresses;
		private List<PhoneNumber> _phoneNumbers;

		[JsonPropertyName("id")]
		public long ID { get; set; }

		[JsonPropertyName("url")]
		public string URL { get => $"/admin/persons/{ID}"; }

		[JsonPropertyName("fullName")]
		public string Name
		{
			get => _name;
			set
			{
				_name = value;
				NameUpdated = true;
			}
		}

		[JsonPropertyName("organizationUrl")]
		public string OrganizationURL
		{
			get => _organizationURL;
			set
			{
				_organizationURL = value;
				OrganizationURLUpdated = true;
			}
		}

		[JsonPropertyName("emails")]
		public List<string> EmailAddresses {
			get => _emailAddresses;
			set
			{
				_emailAddresses = value;
				EmailAddressesUpdated = true;
			}
		}

		[JsonPropertyName("phoneNumbers")]
		public List<PhoneNumber> PhoneNumbers {
			get => _phoneNumbers;
			set
			{
				_phoneNumbers = value;
				PhoneNumbersUpdated = true;
			}
		}

		public bool NameUpdated { get; private set; } = false;
		public bool OrganizationURLUpdated { get; private set; } = false;
		public bool PhoneNumbersUpdated { get; private set; } = false;
		public bool EmailAddressesUpdated { get; private set; } = false;


		public PersonUpdateParameters ToPersonUpdateParameters() => new PersonUpdateParameters
		{
			ID = ID,
			Name = Name,
			OrganizationID = ParseNullableIDFromURL(OrganizationURL),
			PhoneNumbers = PhoneNumbers?.Select(x => (x.Type, x.Number))?.ToList(),
			EmailAddresses = EmailAddresses,
			NameUpdated = NameUpdated,
			OrganizationIDUpdated = OrganizationURLUpdated,
			PhoneNumbersUpdated = PhoneNumbersUpdated,
			EmailAddressesUpdated = EmailAddressesUpdated
		};


		public class PhoneNumber
		{
			[JsonPropertyName("number")]
			public string Number { get; set; }

			[JsonPropertyName("type")]
			public string Type { get; set; }
		}
	}
}
