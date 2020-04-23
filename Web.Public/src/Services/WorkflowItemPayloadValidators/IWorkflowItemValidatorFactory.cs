using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions;

namespace Web.Public.Services.WorkflowItemPayloadValidators
{
	public interface IWorkflowItemValidatorFactory
	{
		IWorkflowItemValidator GetWorkflowItemPayloadValidator(APIAction action);
	}
}