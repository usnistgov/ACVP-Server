using ACVPWorkflow.Providers;
using ACVPWorkflow.Results;

namespace ACVPWorkflow.Services
{
	public class WorkflowService : IWorkflowService
	{
		IWorkflowProvider _workflowProvider;

		public WorkflowService(IWorkflowProvider workflowProvider)
		{
			_workflowProvider = workflowProvider;
		}


		public WorkflowInsertResult CreateDependencyDelete(long dependencyID, WorkflowContact contact)
		{
			string json = $"{{\"url\": \"/dependencies/{dependencyID}\"}}";         //{"url": "/dependencies/<dependenyID>"}
			var result = _workflowProvider.Insert(WorkflowItemType.Dependency, RequestAction.Delete, contact.Lab, contact.Name, contact.Email, json);
			return result;
		}
	}
}
