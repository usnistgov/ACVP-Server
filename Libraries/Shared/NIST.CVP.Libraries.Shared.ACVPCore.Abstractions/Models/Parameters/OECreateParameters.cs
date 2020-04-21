using System.Collections.Generic;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Models.Parameters
{
	public class OECreateParameters
	{
		public string Name { get; set; }
		public List<long> DependencyIDs { get; set; } = new List<long>();
	}
}
