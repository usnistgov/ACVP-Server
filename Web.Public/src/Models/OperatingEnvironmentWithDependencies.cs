using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Web.Public.Models
{
	public class OperatingEnvironmentWithDependencies
	{
		[JsonIgnore]
		public long ID { get; set; }

		[JsonPropertyName("url")]
		public string URL => $"/acvp/v1/oes/{ID}";

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("dependencies")]
		public List<Dependency> Dependencies { get; set; }
	}
}
