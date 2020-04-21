using NIST.CVP.Libraries.Internal.ACVPWorkflow.WorkflowItemProcessors;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow
{
	public interface IWorkflowItemProcessorFactory
	{
		IWorkflowItemProcessor GetWorkflowItemProcessor(APIAction action);
	}
}