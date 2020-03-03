using ACVPCore.Models;
using ACVPCore.Results;
using ACVPWorkflow.Models;
using ACVPWorkflow.Models.Parameters;
using ACVPWorkflow.Results;

namespace ACVPWorkflow.Services
{
	public interface IWorkflowService
	{
		WorkflowInsertResult AddWorkflowItem(APIAction apiAction, long requestID, string payload, long userID);
		Result Validate(WorkflowItem workflowItem);
		Result Approve(WorkflowItem workflowItem);
		Result Reject(WorkflowItem workflowItem);
		PagedEnumerable<WorkflowItemLite> GetWorkflowItems(WorkflowListParameters param);
		WorkflowItem GetWorkflowItem(long workflowItemId);
	}
}