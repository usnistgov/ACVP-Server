using System.Collections.Generic;

namespace ACVPCore.Models.Parameters
{
	public class ImplementationUpdateParameters
	{
		public long ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Version { get; set; }
		public ImplementationType Type { get; set; }
		public string Website { get; set; }
		public long? OrganizationID { get; set; }
		public long? AddressID { get; set; }
		public List<long> ContactIDs { get; set; }

		public bool NameUpdated { get; set; }
		public bool DescriptionUpdated { get; set; }
		public bool TypeUpdated { get; set; }
		public bool VersionUpdated { get; set; }
		public bool WebsiteUpdated { get; set; }
		public bool OrganizationIDUpdated { get; set; }
		public bool AddressIDUpdated { get; set; }
		public bool ContactIDsUpdated { get; set; }
	}
}
