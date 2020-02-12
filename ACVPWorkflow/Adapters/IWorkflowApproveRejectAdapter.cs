using ACVPWorkflow.Results;

namespace ACVPWorkflow.Adapters
{
    /// <summary>
    /// Adapter to allow the approval/rejection of a workflow item by its ID.
    /// </summary>
    public interface IWorkflowApproveRejectAdapter
    {
        /// <summary>
        /// Approve workflow item.
        /// </summary>
        /// <param name="workflowId">The workflow ID to approve.</param>
        /// <returns>Result</returns>
        Result Approve(long workflowId);
        /// <summary>
        /// Deny workflow item.
        /// </summary>
        /// <param name="workflowId">The workflow ID to deny.</param>
        /// <returns>Result</returns>
        Result Reject(long workflowId);
    }
}