using ACVPCore.Models.Parameters;

namespace ACVPWorkflow.Models.Parameters
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
    }
}