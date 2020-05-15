using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;

namespace NIST.CVP.Libraries.Shared.MessageQueue.Abstractions
{
	public interface IWorkflowItemProcessor
	{
		bool Validate(WorkflowItem workflowItem);
		long Approve(WorkflowItem workflowItem);
		void Reject(WorkflowItem workflowItem);
	}
}
