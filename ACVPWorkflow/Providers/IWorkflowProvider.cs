using ACVPWorkflow.Results;

namespace ACVPWorkflow.Providers
{
	public interface IWorkflowProvider
	{
		WorkflowInsertResult Insert(WorkflowItemType workflowItemType, RequestAction action, string labName, string contact, string email, string json);
	}
}