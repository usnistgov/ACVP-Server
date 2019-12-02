using ACVPWorkflow.Results;

namespace ACVPWorkflow.Services
{
	public interface IWorkflowService
	{
		WorkflowInsertResult CreateDependencyDelete(long dependencyID, WorkflowContact contact);
	}
}