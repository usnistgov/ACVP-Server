using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;

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
