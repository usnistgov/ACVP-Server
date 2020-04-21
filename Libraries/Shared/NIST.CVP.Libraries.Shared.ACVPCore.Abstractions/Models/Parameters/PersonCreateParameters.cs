using System.Collections.Generic;

namespace NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters
{
	public class PersonCreateParameters
	{
		public string Name { get; set; }
		public long OrganizationID { get; set; }
		public List<(string Type, string Number)> PhoneNumbers { get; set; } = new List<(string Type, string Number)>();
		public List<string> EmailAddresses { get; set; } = new List<string>();
	}
}
