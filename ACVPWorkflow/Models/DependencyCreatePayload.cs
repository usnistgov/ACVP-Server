using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ACVPWorkflow.Models
{
	public class DependencyCreatePayload
	{
		[JsonPropertyName("id")]
		public long ID { get => -1; }

		[JsonPropertyName("url")]
		public string URL { get => "/admin/dependencies/-1"; }

		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("description")]
		public string Description { get; set; }

		//Since the dependency attributes do not have standard names, and are just key/value pair items, use the JsonExtensionData thing to capture all of the attribute data
		[JsonExtensionData]
		public Dictionary<string, JsonElement> Attributes { get; set; }
	}
}
