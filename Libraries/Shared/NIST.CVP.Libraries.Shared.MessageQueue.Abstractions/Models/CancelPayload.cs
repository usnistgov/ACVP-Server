using System.Text.Json.Serialization;

namespace NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models
{
	public class CancelPayload : IWorkflowItemPayload
	{
		[JsonPropertyName("tsId")]
		public long TestSessionID { get; set; }
		[JsonPropertyName("vsId")]
		public long VectorSetID { get; set; }
	}
}
