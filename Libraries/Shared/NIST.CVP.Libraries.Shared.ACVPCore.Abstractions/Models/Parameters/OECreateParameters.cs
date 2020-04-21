using System.Collections.Generic;

namespace NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters
{
	public class OECreateParameters
	{
		public string Name { get; set; }
		public List<long> DependencyIDs { get; set; } = new List<long>();
	}
}
