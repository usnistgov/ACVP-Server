using System.Collections.Generic;

namespace ACVPCore.Models
{
	public class Dependency
	{
		public long ID { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
		public string Description { get; set; }
		public List<DependencyAttribute> Attributes { get; set; } = new List<DependencyAttribute>();
	}
}
