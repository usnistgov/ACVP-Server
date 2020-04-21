using NIST.CVP.Libraries.Internal.ACVPWorkflow.Models;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.WorkflowItemPayloadValidators
{
	public interface IWorkflowItemPayloadValidator
	{
		public bool Validate(IWorkflowItemPayload workflowItemPayload);
	}
}
