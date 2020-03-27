using ACVPWorkflow.Models;

namespace ACVPWorkflow.WorkflowItemPayloadValidators
{
	public class CreateDependencyPayloadValidator : IWorkflowItemPayloadValidator
	{
		public CreateDependencyPayloadValidator()
		{
		}

		public bool Validate(IWorkflowItemPayload workflowItemPayload)
		{
			//Not really anything to validate...
			return true;
		}
	}
}
