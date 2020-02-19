using System.Collections.Generic;
using ACVPCore.Results;
using ACVPWorkflow.Models;
using ACVPWorkflow.Results;

namespace ACVPWorkflow.Services
{
	public interface IWorkflowService
	{
		WorkflowInsertResult AddWorkflowItem(APIAction apiAction, long requestID, IWorkflowItemPayload payload, long userID);
		Result Validate(WorkflowItem workflowItem);
		Result Approve(WorkflowItem workflowItem);
		Result Reject(WorkflowItem workflowItem);
		List<WorkflowItemLite> GetWorkflowItems(WorkflowStatus status);
		WorkflowItem GetWorkflowItem(long workflowItemId);
	}
}