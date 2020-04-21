using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Exceptions;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models;

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
