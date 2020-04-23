using System.Text.Json.Serialization;

namespace NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models
{
	public class CancelPayload
	{
		[JsonPropertyName("tsId")]
		public long TestSessionID { get; set; }
		[JsonPropertyName("vsId")]
		public long VectorSetID { get; set; }
	}
}
