using System.Text.Json.Serialization;

namespace JwtCreatinator.Models
{
	public class JwtRenewRequest
	{
		[JsonPropertyName("testSessionId")]
		public long TestSessionId { get; set; }
		[JsonPropertyName("clientCertSubject")]
		public string ClientCertSubject { get; set; }
	}
}