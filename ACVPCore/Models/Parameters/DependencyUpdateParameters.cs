using System.Collections.Generic;

namespace ACVPCore.Models.Parameters
{
	public class DependencyUpdateParameters
	{
		public long ID { get; set; }
		public string Type { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public List<DependencyAttributeCreateParameters> Attributes { get; set; } = new List<DependencyAttributeCreateParameters>();


		public bool TypeUpdated { get; set; }
		public bool NameUpdated { get; set; }
		public bool DescriptionUpdated { get; set; }
		public bool AttributesUpdated { get; set; }
	}
}
