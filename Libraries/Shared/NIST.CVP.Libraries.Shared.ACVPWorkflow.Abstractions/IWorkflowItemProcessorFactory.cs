namespace NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions
{
	public interface IWorkflowItemProcessorFactory
	{
		IWorkflowItemProcessor GetWorkflowItemProcessor(APIAction action);
	}
}