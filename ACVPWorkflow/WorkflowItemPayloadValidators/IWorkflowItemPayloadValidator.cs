using ACVPWorkflow.Models;

namespace ACVPWorkflow.WorkflowItemPayloadValidators
{
	public interface IWorkflowItemPayloadValidator
	{
		public bool Validate(IWorkflowItemPayload workflowItemPayload);
	}
}
