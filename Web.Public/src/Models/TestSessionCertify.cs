using System.Collections.Generic;
using System.Text.Json.Serialization;
using Web.Public.JsonObjects;

namespace Web.Public.Models
{
	public class TestSessionCertify : IJsonObject
	{
		[JsonPropertyName("testSessionId")]
		public long TestSessionId { get; set; }
		
		[JsonPropertyName("moduleUrl")]
		public string ModuleUrl { get; set; }

		[JsonPropertyName("module")]
		public Implementation Implementation { get; set; }
		
		[JsonPropertyName("oeUrl")]
		public string OeUrl { get; set; }

		[JsonPropertyName("oe")]
		public OperatingEnvironment OperatingEnvironment { get; set; }
		
		[JsonExtensionData]
		public IDictionary<string, object> Properties { get; set; }
		
		public List<string> ValidateObject()
		{
			return new List<string>();
		}
	}
}