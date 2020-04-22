using System.Text.Json.Serialization;

namespace ACVPWorkflow.Models
{
	public class WorkflowItem
	{
		public long WorkflowItemID { get; set; }
		public long RequestId { get; set; }
		public APIAction APIAction { get; set; }

		[JsonConverter(typeof(WorkflowItemPayloadConverter))]
		public IWorkflowItemPayload Payload { get; set; }
		public WorkflowStatus Status { get; set; }
	}
}
