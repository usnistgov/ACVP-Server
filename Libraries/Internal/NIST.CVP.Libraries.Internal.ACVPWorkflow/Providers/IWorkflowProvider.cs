using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Results;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.Providers
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