using System;
using System.Text.Json;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using ACVPWorkflow.Services;

namespace ACVPWorkflow.WorkflowItemProcessors
{
	public class CreateImplementationProcessor : IWorkflowItemProcessor
	{
		private readonly IImplementationService _implementationService;
		private readonly IWorkflowService _workflowService;

		public CreateImplementationProcessor(IImplementationService implementationService, IWorkflowService workflowService)
		{
			_implementationService = implementationService;
			_workflowService = workflowService;
		}

		public void Approve(WorkflowItem workflowItem)
		{
			ImplementationCreateParameters parameters = JsonSerializer.Deserialize<ImplementationCreateParameters>(workflowItem.JSON);

			//Create it
			ImplementationResult implementationCreateResult = _implementationService.Create(parameters);

			//Update the workflow item
			if (implementationCreateResult.IsSuccess)
			{
				_workflowService.MarkApproved(workflowItem.WorkflowItemID, implementationCreateResult.ID);
			}
		}

		public void Reject(WorkflowItem workflowItem)
		{
			throw new NotImplementedException();
		}
	}
}
