using System.Text.Json.Serialization;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.Models
{
	public class WorkflowItem
	{
		public long WorkflowItemID { get; set; }
		public APIAction APIAction { get; set; }

		[JsonConverter(typeof(WorkflowItemPayloadConverter))]
		public IWorkflowItemPayload Payload { get; set; }
		public WorkflowStatus Status { get; set; }
	}
}
