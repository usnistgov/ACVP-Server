using System;
using System.Text.Json;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using ACVPWorkflow.Models;
using ACVPWorkflow.Services;

namespace ACVPWorkflow.WorkflowItemProcessors
{
	public class DeletePersonProcessor : IWorkflowItemProcessor
	{
		private readonly IPersonService _personService;
		private readonly IWorkflowService _workflowService;

		public DeletePersonProcessor(IPersonService personService, IWorkflowService workflowService)
		{
			_personService = personService;
			_workflowService = workflowService;
		}

		public void Approve(WorkflowItem workflowItem)
		{
			//Deserialize the JSON
			DeleteParameters deleteParameters = JsonSerializer.Deserialize<DeletePayload>(workflowItem.JSON).ToDeleteParameters();

			//Delete that person - will fail if person is in use
			DeleteResult deleteResult = _personService.Delete(deleteParameters.ID);

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
