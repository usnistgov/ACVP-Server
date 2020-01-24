using System.Collections.Generic;
using System.Text.Json.Serialization;
using ACVPCore.Models.Parameters;

namespace ACVPWorkflow.Models
{
	public class OECreatePayload : BasePayload
	{
		[JsonPropertyName("id")]
		public long ID { get => -1; }

		[JsonPropertyName("url")]
		public string URL { get => "/admin/oes/-1"; }

		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("description")]
		public string Description { get; set; }

		[JsonPropertyName("dependencyUrls")]
		public List<string> DependencyURLs { get; set; }

		public OECreateParameters ToOECreateParameters() => new OECreateParameters
		{
			Name = Name,
			DependencyIDs = DependencyURLs.ConvertAll<long>(x => ParseIDFromURL(x))
		};
	}
}
