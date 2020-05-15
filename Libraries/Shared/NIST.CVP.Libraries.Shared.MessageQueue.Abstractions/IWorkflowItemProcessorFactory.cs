namespace NIST.CVP.Libraries.Shared.MessageQueue.Abstractions
{
	public interface IWorkflowItemProcessorFactory
	{
		IWorkflowItemProcessor GetWorkflowItemProcessor(APIAction action);
	}
}