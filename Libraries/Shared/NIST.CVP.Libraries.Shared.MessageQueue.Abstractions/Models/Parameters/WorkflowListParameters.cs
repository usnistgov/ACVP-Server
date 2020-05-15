using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;

namespace NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models.Parameters
{
    /// <summary>
    /// Provides searching and paging capabilities when pulling a Workflow list.
    /// </summary>
    public class WorkflowListParameters : PagedParametersBase
    {
        /// <summary>
        /// The workflow item ID to search for.
        /// </summary>
        public long? WorkflowItemId { get; set; }
        /// <summary>
        /// The API Action type to search for.
        /// </summary>
        public APIAction? APIActionId { get; set; }
        /// <summary>
        /// The requestId of the workflow item.
        /// </summary>
        public string RequestId { get; set; }
        /// <summary>
        /// The status of the workflow item.
        /// </summary>
        public WorkflowStatus? Status { get; set; }
    }
}