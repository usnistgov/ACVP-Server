using NIST.CVP.Libraries.Internal.ACVPWorkflow.Models;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.WorkflowItemProcessors
{
	public interface IWorkflowItemProcessor
	{
		bool Validate(WorkflowItem workflowItem);
		long Approve(WorkflowItem workflowItem);
		void Reject(WorkflowItem workflowItem);
	}
}
