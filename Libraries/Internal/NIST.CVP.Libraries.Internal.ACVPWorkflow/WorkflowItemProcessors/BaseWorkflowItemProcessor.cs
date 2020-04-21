using NIST.CVP.Libraries.Internal.ACVPWorkflow.Exceptions;
using NIST.CVP.Libraries.Internal.ACVPWorkflow.Models;

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
