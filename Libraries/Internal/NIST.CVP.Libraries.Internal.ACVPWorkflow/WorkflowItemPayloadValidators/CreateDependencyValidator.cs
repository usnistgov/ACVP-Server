using NIST.CVP.Libraries.Internal.ACVPWorkflow.Models;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.WorkflowItemPayloadValidators
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
