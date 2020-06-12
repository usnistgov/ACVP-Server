using System.Text.Json.Serialization;

namespace Web.Public.Models
{
	public class Validation
	{
		[JsonIgnore]
		public long Id { get; set; }

		public string Url => $"/acvp/v1/validations/{Id}";

		[JsonIgnore]
		public string SourcePrefix { get; set; }
		[JsonIgnore]
		public long ValidationId { get; set; }
		
		[JsonPropertyName("validationId")]
		public string ReturnToClientThisId => $"{SourcePrefix}{ValidationId}";
	}
}