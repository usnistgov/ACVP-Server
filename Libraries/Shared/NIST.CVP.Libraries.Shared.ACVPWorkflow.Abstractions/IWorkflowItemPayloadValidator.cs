using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models;

namespace NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions
{
	public interface IWorkflowItemPayloadValidator
	{
		public bool Validate(IWorkflowItemPayload workflowItemPayload);
	}
}
