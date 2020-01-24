using System;
using System.Text.Json;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using ACVPWorkflow.Models;
using ACVPWorkflow.Services;

namespace ACVPWorkflow.WorkflowItemProcessors
{
	public class DeleteImplementationProcessor : IWorkflowItemProcessor
	{
		private readonly IImplementationService _implementationService;
		private readonly IWorkflowService _workflowService;

		public DeleteImplementationProcessor(IImplementationService implementationService, IWorkflowService workflowService)
		{
			_implementationService = implementationService;
			_workflowService = workflowService;
		}

		public void Approve(WorkflowItem workflowItem)
		{
			//Deserialize the JSON
			DeleteParameters deleteParameters = JsonSerializer.Deserialize<DeletePayload>(workflowItem.JSON).ToDeleteParameters();

			//Delete that implementation - will fail if implementation is in use
			DeleteResult deleteResult = _implementationService.Delete(deleteParameters.ID);

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
