using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Web.Public.JsonObjects;

namespace Web.Public.Models
{
	public class Dependency
	{
		[JsonIgnore]
		public long ID { get; set; }

		[JsonPropertyName("url")]
		public string URL => $"/acvp/v1/dependencies/{ID}";

		[JsonPropertyName("type")]
		public string DependencyType { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("description")]
		public string Description { get; set; }

		[JsonExtensionData]
		public Dictionary<string, object> Attributes { get; set; }

		public List<string> ValidateObject()
		{
			var errors = new List<string>();
			
			//TODO - something
			
			return errors;
		}
	}
}
