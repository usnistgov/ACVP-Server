using System.Collections.Generic;

namespace NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters
{
	public class DependencyCreateParameters
	{
		public string Type { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public List<DependencyAttributeCreateParameters> Attributes { get; set; } = new List<DependencyAttributeCreateParameters>();
	}
}
