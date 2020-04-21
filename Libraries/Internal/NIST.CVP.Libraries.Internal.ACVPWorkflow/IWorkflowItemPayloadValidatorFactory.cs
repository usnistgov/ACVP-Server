using NIST.CVP.Libraries.Internal.ACVPWorkflow.WorkflowItemPayloadValidators;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow
{
	public interface IWorkflowItemPayloadValidatorFactory
	{
		IWorkflowItemPayloadValidator GetWorkflowItemPayloadValidator(APIAction action);
	}
}