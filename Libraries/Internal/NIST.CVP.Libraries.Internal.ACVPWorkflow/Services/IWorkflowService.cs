using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Results;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.Services
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