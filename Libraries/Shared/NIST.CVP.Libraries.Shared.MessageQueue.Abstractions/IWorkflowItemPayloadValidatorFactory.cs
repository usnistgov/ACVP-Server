namespace NIST.CVP.Libraries.Shared.MessageQueue.Abstractions
{
	public interface IWorkflowItemPayloadValidatorFactory
	{
		IWorkflowItemPayloadValidator GetWorkflowItemPayloadValidator(APIAction action);
	}
}