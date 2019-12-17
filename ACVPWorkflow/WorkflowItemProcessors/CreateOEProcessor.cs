using System;
using System.Text.Json;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using ACVPWorkflow.Services;

namespace ACVPWorkflow.WorkflowItemProcessors
{
	public class CreateOEProcessor : IWorkflowItemProcessor
	{
		private readonly IOEService _oeService;
		private readonly IWorkflowService _workflowService;

		public CreateOEProcessor(IOEService oeService, IWorkflowService workflowService)
		{
			_oeService = oeService;
			_workflowService = workflowService;
		}

		public void Approve(WorkflowItem workflowItem)
		{
			OECreateParameters parameters = JsonSerializer.Deserialize<OECreateParameters>(workflowItem.JSON);

			//Create it
			OEResult oeCreateResult = _oeService.Create(parameters);

			//Update the workflow item
			if (oeCreateResult.IsSuccess)
			{
				_workflowService.MarkApproved(workflowItem.WorkflowItemID, oeCreateResult.ID);
			}
		}

		public void Reject(WorkflowItem workflowItem)
		{
			throw new NotImplementedException();
		}
	}
}
