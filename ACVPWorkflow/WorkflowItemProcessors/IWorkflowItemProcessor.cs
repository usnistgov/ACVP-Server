using ACVPWorkflow.Models;

namespace ACVPWorkflow.WorkflowItemProcessors
{
	/// <summary>
	/// Interface for processing workflow items.
	/// </summary>
	public interface IWorkflowItemProcessor
	{
		/// <summary>
		/// Approve a workflow item, returning the ID of the newly created item.
		/// </summary>
		/// <param name="workflowItem">The workflow item to create.</param>
		/// <returns>The ID of the created resource.</returns>
		long Approve(WorkflowItem workflowItem);
		/// <summary>
		/// Reject a workflow item.
		/// </summary>
		/// <param name="workflowItem">The workflow item to reject.</param>
		void Reject(WorkflowItem workflowItem);
	}
}
