using ACVPWorkflow.Models;
using ACVPWorkflow.Results;

namespace ACVPWorkflow.Services
{
	public interface IWorkflowService
	{
		WorkflowInsertResult AddWorkflowItem(APIAction apiAction, long requestID, IWorkflowItemPayload payload, long userID);
		Result UpdateStatus(long workflowItemID, WorkflowStatus workflowStatus);
		Result MarkApproved(long workflowItemID, long objectID);
	}
}