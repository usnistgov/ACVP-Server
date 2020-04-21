namespace NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions
{
	public interface IWorkflowItemPayloadValidatorFactory
	{
		IWorkflowItemPayloadValidator GetWorkflowItemPayloadValidator(APIAction action);
	}
}