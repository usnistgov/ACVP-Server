namespace ACVPWorkflow.Models
{
	public class WorkflowItem
	{
		public long WorkflowItemID { get; set; }
		public APIAction APIAction { get; set; }
		public IWorkflowItemPayload Payload { get; set; }
		public WorkflowStatus Status { get; set; }
	}
}
