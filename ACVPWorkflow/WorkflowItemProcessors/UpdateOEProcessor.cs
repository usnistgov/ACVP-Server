using System;
using System.Text.Json;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using ACVPWorkflow.Models;
using ACVPWorkflow.Services;

namespace ACVPWorkflow.WorkflowItemProcessors
{
	public class UpdateOEProcessor : IWorkflowItemProcessor
	{
		private readonly IOEService _oeService;
		private readonly IWorkflowService _workflowService;

		public UpdateOEProcessor(IOEService oeService, IWorkflowService workflowService)
		{
			_oeService = oeService;
			_workflowService = workflowService;
		}

		public void Approve(WorkflowItem workflowItem)
		{
			OEUpdateParameters parameters = JsonSerializer.Deserialize<OEUpdatePayload>(workflowItem.JSON).ToOEUpdateParameters();

			//Update it
			OEResult oeUpdateResult = _oeService.Update(parameters);

			//Update the workflow item
			if (oeUpdateResult.IsSuccess)
			{
				_workflowService.MarkApproved(workflowItem.WorkflowItemID, oeUpdateResult.ID);
			}
		}

		public void Reject(WorkflowItem workflowItem)
		{
			throw new NotImplementedException();
		}
	}
}
