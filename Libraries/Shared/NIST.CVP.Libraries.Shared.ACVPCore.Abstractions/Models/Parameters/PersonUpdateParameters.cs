using System.Collections.Generic;

namespace NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters
{
	public class PersonUpdateParameters
	{
		public long ID { get; set; }
		public string Name { get; set; }
		public long? OrganizationID { get; set; }
		public List<(string Type, string Number)> PhoneNumbers { get; set; }
		public List<string> EmailAddresses { get; set; }


		public bool NameUpdated { get; set; }
		public bool OrganizationIDUpdated { get; set; }
		public bool PhoneNumbersUpdated { get; set; }
		public bool EmailAddressesUpdated { get; set; }
	}
}
