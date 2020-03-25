using ACVPWorkflow.WorkflowItemProcessors;

namespace ACVPWorkflow
{
	public interface IWorkflowItemProcessorFactory
	{
		IWorkflowItemProcessor GetWorkflowItemProcessor(APIAction action);
	}
}