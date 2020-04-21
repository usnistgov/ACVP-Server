using System.Collections.Generic;

namespace NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters
{
	public class OrganizationCreateParameters
	{
		public string Name { get; set; }
		public string Website { get; set; }
		public string VoiceNumber { get; set; }
		public string FaxNumber { get; set; }
		public long? ParentOrganizationID { get; set; }
		public List<string> EmailAddresses { get; set; } = new List<string>();
		public List<AddressCreateParameters> Addresses { get; set; } = new List<AddressCreateParameters>();
	}
}
