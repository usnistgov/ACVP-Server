using System.Collections.Generic;
using ACVPCore.Models;
using ACVPWorkflow.Models;
using ACVPWorkflow.Models.Parameters;
using ACVPWorkflow.Results;

namespace ACVPWorkflow.Services
{
	public interface IWorkflowService
	{
		WorkflowInsertResult AddWorkflowItem(APIAction apiAction, long requestID, IWorkflowItemPayload payload, long userID);
		Result Approve(WorkflowItem workflowItem);
		Result Reject(WorkflowItem workflowItem);
		PagedEnumerable<WorkflowItemLite> GetWorkflowItems(WorkflowListParameters param);
		WorkflowItem GetWorkflowItem(long workflowItemId);
	}
}