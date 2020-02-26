using ACVPWorkflow.Exceptions;
using ACVPWorkflow.Models;

namespace ACVPWorkflow.WorkflowItemProcessors
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
