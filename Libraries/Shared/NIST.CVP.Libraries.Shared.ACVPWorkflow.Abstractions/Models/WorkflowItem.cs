using System.Text.Json.Serialization;

namespace NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models
{
	public class WorkflowItem
	{
		public long WorkflowItemID { get; set; }
		public long RequestId { get; set; }
		public APIAction APIAction { get; set; }

		[JsonConverter(typeof(WorkflowItemPayloadConverter))]
		public IWorkflowItemPayload Payload { get; set; }
		public WorkflowStatus Status { get; set; }
		public long? AcceptId { get; set; }
	}
}
