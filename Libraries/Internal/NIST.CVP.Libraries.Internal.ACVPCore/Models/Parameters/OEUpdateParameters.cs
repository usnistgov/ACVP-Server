using System.Collections.Generic;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Models.Parameters
{
	public class OEUpdateParameters
	{
		public long ID { get; set; }
		public string Name { get; set; }
		public List<long> DependencyIDs { get; set; }

		public bool NameUpdated { get; set; }
		public bool DependenciesUpdated { get; set; }
	}
}
