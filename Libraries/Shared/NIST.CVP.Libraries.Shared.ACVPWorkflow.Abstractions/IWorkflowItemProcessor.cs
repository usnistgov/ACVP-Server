using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models;

namespace NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions
{
	public interface IWorkflowItemProcessor
	{
		bool Validate(WorkflowItem workflowItem);
		long Approve(WorkflowItem workflowItem);
		void Reject(WorkflowItem workflowItem);
	}
}
