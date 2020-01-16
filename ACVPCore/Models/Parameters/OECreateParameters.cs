using System.Collections.Generic;

namespace ACVPCore.Models.Parameters
{
	public class OECreateParameters
	{
		public string Name { get; set; }
		public List<string> DependencyURLs { get; set; }
		public List<long> DependencyIDs => DependencyURLs.ConvertAll<long>(x => long.Parse(x.Split("/")[^1]));
	}
}
