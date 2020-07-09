using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Web.Public.Models
{
	public class Validation
	{
		[JsonIgnore]
		public long Id { get; set; }

		[JsonPropertyName("url")]
		public string Url => $"/acvp/v1/validations/{Id}";

		[JsonIgnore]
		public string SourcePrefix { get; set; }

		[JsonIgnore]
		public long ValidationNumber { get; set; }
		
		[JsonPropertyName("validationId")]
		public string ReturnToClientThisId => $"{SourcePrefix}{ValidationNumber}";

		[JsonIgnore]
		public long ImplementationID { get; set; }

		[JsonPropertyName("moduleUrl")]
		public string ImplementationUrl => $"/acvp/v1/modules/{ImplementationID}";

		[JsonIgnore]
		public List<long> OEIDs { get; set; }

		[JsonPropertyName("oeUrls")]
		public IEnumerable<string> OEUrls => OEIDs.Select(x => $"/acvp/v1/oes/{x}");
	}
}