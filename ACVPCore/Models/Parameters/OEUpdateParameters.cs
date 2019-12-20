using System.Collections.Generic;

namespace ACVPCore.Models.Parameters
{
	public class OEUpdateParameters
	{
		public long ID { get; set; }
		public string Name { get; set; }
		public List<string> DependencyURLs { get; set; } = new List<string>();
		public List<long> DependencyIDs => DependencyURLs.ConvertAll<long>(x => long.Parse(x.Split("/")[^1]));

		public bool NameUpdated { get; set; }
		public bool DependenciesUpdated { get; set; }
	}
}
