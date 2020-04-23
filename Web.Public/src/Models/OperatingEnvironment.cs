using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Web.Public.JsonObjects;

namespace Web.Public.Models
{
	public class OperatingEnvironment
	{
		[JsonIgnore]
		public long ID { get; set; }

		[JsonPropertyName("url")]
		public string URL => $"/acvp/v1/oes/{ID}";

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonIgnore]
		public List<long> DependencyIDs { get; set; }

		[JsonPropertyName("dependencyUrls")]
		public List<string> VendorURL => DependencyIDs.Select(x => $"/acvp/v1/dependencies/{x}").ToList();

		public List<string> ValidateObject()
		{
			var errors = new List<string>();
			
			//TODO - something
			
			return errors;
		}
	}
}
