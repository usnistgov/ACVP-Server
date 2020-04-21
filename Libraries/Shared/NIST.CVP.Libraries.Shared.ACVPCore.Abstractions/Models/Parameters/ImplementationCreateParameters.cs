using System.Collections.Generic;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Models.Parameters
{
	public class ImplementationCreateParameters
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string Version { get; set; }
		public ImplementationType Type { get; set; }
		public string Website { get; set; }
		public long OrganizationID { get; set; }
		public long? AddressID { get; set; }
		public List<long> ContactIDs { get; set; } = new List<long>();
		public bool IsITAR { get; set; }
	}
}
