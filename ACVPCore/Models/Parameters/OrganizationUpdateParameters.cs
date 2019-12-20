using System.Collections.Generic;

namespace ACVPCore.Models.Parameters
{
	public class OrganizationUpdateParameters
	{
		public long ID { get; set; }
		public string Name { get; set; }
		public string Website { get; set; }
		public string VoiceNumber { get; set; }
		public string FaxNumber { get; set; }
		public long? ParentOrganizationID { get; set; }
		public List<string> EmailAddresses { get; set; }
		public List<object> Addresses { get; set; }


		public bool NameUpdated { get; set; }
		public bool WebsiteUpdated { get; set; }
		public bool ParentOrganizationIDUpdated { get; set; }
		public bool PhoneNumbersUpdated { get; set; }
		public bool EmailAddressesUpdated { get; set; }
		public bool AddressesUpdated { get; set; }
	}
}
