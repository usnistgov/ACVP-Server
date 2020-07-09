using System.Text.Json.Serialization;

namespace JwtCreatinator.Models
{
	public class JwtRenewResponse
	{
		[JsonPropertyName("testSessionId")]
		public long TestSessionId { get; set; }
		[JsonPropertyName("jwt")]
		public string Jwt { get; set; }
	}
}