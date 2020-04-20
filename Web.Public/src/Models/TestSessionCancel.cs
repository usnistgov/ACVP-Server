using System.Text.Json.Serialization;

namespace Web.Public.Models
{
	public class TestSessionCancel
	{
		[JsonPropertyName("tsId")]
		public long TestSessionId { get; set; }
	}
}