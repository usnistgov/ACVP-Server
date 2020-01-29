using System;
using System.Text.Json;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using ACVPWorkflow.Models;
using ACVPWorkflow.Services;

namespace ACVPWorkflow.WorkflowItemProcessors
{
	public class UpdateImplementationProcessor : IWorkflowItemProcessor
	{
		private readonly IImplementationService _implementationService;
		private readonly IWorkflowService _workflowService;

		public UpdateImplementationProcessor(IImplementationService implementationService, IWorkflowService workflowService)
		{
			_implementationService = implementationService;
			_workflowService = workflowService;
		}

		public void Approve(WorkflowItem workflowItem)
		{
			ImplementationUpdateParameters parameters = ((ImplementationUpdatePayload)workflowItem.Payload).ToImplementationUpdateParameters();

			//Update it
			ImplementationResult implementationUpdateResult = _implementationService.Update(parameters);

			//Update the workflow item
			if (implementationUpdateResult.IsSuccess)
			{
				_workflowService.MarkApproved(workflowItem.WorkflowItemID, implementationUpdateResult.ID);
			}
		}

		public void Reject(WorkflowItem workflowItem)
		{
			throw new NotImplementedException();
		}
	}
}
