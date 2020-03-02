using ACVPWorkflow.WorkflowItemPayloadValidators;

namespace ACVPWorkflow
{
	public interface IWorkflowItemPayloadValidatorFactory
	{
		IWorkflowItemPayloadValidator GetWorkflowItemPayloadValidator(APIAction action);
	}
}