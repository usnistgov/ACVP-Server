using System.Collections.Generic;
using ACVPWorkflow.Models;
using ACVPWorkflow.Results;

namespace ACVPWorkflow.Providers
{
	public interface IWorkflowProvider
	{
		WorkflowInsertResult Insert(APIAction apiAction, WorkflowItemType workflowItemType, RequestAction action, long userID, string json, string labName, string contact, string email);
		Result Update(long workflowItemID, WorkflowStatus status, long acceptID);
		Result Update(long workflowItemID, WorkflowStatus status);
		List<WorkflowItemLite> GetWorkflowItems(WorkflowStatus status);
		WorkflowItem GetWorkflowItem(long workflowItemId);
	}
}