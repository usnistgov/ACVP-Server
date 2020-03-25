using ACVPWorkflow.Models;
using ACVPWorkflow.Models.Parameters;
using ACVPWorkflow.Results;
using NIST.CVP.Enumerables;
using NIST.CVP.Results;

namespace ACVPWorkflow.Providers
{
	public interface IWorkflowProvider
	{
		WorkflowInsertResult Insert(APIAction apiAction, WorkflowItemType workflowItemType, RequestAction action, long userID, string json, string labName, string contact, string email);
		Result Update(long workflowItemID, WorkflowStatus status, long acceptID);
		Result Update(long workflowItemID, WorkflowStatus status);
		PagedEnumerable<WorkflowItemLite> GetWorkflowItems(WorkflowListParameters param);
		WorkflowItem GetWorkflowItem(long workflowItemId);
	}
}