using ACVPWorkflow.Models;

namespace ACVPWorkflow.WorkflowItemProcessors
{
	public interface IWorkflowItemProcessor
	{
		void Approve(WorkflowItem workflowItem);
		void Reject(WorkflowItem workflowItem);
	}
}
