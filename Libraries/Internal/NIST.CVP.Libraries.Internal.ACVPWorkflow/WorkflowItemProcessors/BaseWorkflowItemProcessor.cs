using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Exceptions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.WorkflowItemProcessors
{
	public class BaseWorkflowItemProcessor
	{
		public static bool IsPendingApproval(WorkflowItem workflowItem)
		{
			if (workflowItem.Status != WorkflowStatus.Pending)
			{
				throw new NotPendingApprovalException("Workflow item is not pending approval");
			}

			return true;
		}
	}
}
