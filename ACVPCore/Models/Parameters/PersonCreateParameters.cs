using System.Collections.Generic;

namespace ACVPCore.Models.Parameters
{
	public class PersonCreateParameters
	{
		public string Name { get; set; }
		public long OrganizationID { get; set; }
		public List<(string Type, string Number)> PhoneNumbers { get; set; }
		public List<string> EmailAddresses { get; set; }
	}
}
