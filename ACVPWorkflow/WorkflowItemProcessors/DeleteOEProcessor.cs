using System;
using System.Text.Json;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using ACVPWorkflow.Services;

namespace ACVPWorkflow.WorkflowItemProcessors
{
	public class DeleteOEProcessor : IWorkflowItemProcessor
	{
		private readonly IOEService _oeService;
		private readonly IWorkflowService _workflowService;

		public DeleteOEProcessor(IOEService oeService, IWorkflowService workflowService)
		{
			_oeService = oeService;
			_workflowService = workflowService;
		}

		public void Approve(WorkflowItem workflowItem)
		{
			//Deserialize the JSON to get the ID, as that's all that's there
			DeleteParameters deletePayload = JsonSerializer.Deserialize<DeleteParameters>(workflowItem.JSON);

			//Delete that oe - will fail if oe is in use
			DeleteResult deleteResult = _oeService.Delete(deletePayload.ID);

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
