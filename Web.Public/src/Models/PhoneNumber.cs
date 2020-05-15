using System.Text.Json.Serialization;

namespace Web.Public.Models
{
	public class PhoneNumber
	{
		[JsonPropertyName("number")]
		public string Number { get; set; }

		[JsonPropertyName("type")]
		public string Type { get; set; }
	}
}
