using ACVPWorkflow.Models;

namespace ACVPWorkflow.WorkflowItemProcessors
{
	public interface IWorkflowItemProcessor
	{
		bool Validate(WorkflowItem workflowItem);
		long Approve(WorkflowItem workflowItem);
		void Reject(WorkflowItem workflowItem);
	}
}
