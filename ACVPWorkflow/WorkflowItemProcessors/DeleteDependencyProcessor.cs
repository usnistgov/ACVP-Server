using System;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using ACVPWorkflow.Models;
using ACVPWorkflow.Services;

namespace ACVPWorkflow.WorkflowItemProcessors
{
	public class DeleteDependencyProcessor : IWorkflowItemProcessor
	{
		private readonly IDependencyService _dependencyService;
		private readonly IWorkflowService _workflowService;

		public DeleteDependencyProcessor(IDependencyService dependencyService, IWorkflowService workflowService)
		{
			_dependencyService = dependencyService;
			_workflowService = workflowService;
		}

		public void Approve(WorkflowItem workflowItem)
		{
			//Deserialize the JSON
			DeleteParameters deleteParameters = ((DeletePayload)workflowItem.Payload).ToDeleteParameters();

			//Delete that dependency - will fail if dependency is in use
			DeleteResult deleteResult = _dependencyService.Delete(deleteParameters.ID);

			if (deleteResult.IsSuccess)
			{
				//Update status to approved
				_workflowService.UpdateStatus(workflowItem.WorkflowItemID, WorkflowStatus.Approved);
			}
			else
			{
				if (deleteResult.IsInUse)
				{
					//Mark rejected
					_workflowService.UpdateStatus(workflowItem.WorkflowItemID, WorkflowStatus.Rejected);
				}
				else
				{
					//Mark as error? No way to do so, just in process
					// TODO - something here
				}
			}
		}

		public void Reject(WorkflowItem workflowItem)
		{
			throw new NotImplementedException();
		}
	}
}
