using System;
using System.Text.Json;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using ACVPWorkflow.Services;

namespace ACVPWorkflow.WorkflowItemProcessors
{
	public class CreateDependencyProcessor : IWorkflowItemProcessor
	{
		private readonly IDependencyService _dependencyService;
		private readonly IWorkflowService _workflowService;

		public CreateDependencyProcessor(IDependencyService dependencyService, IWorkflowService workflowService)
		{
			_dependencyService = dependencyService;
			_workflowService = workflowService;
		}

		public void Approve(WorkflowItem workflowItem)
		{
			DependencyCreateParameters parameters = JsonSerializer.Deserialize<DependencyCreateParameters>(workflowItem.JSON);

			//Create it
			DependencyResult dependencyCreateResult = _dependencyService.Create(parameters);

			//Update the workflow item
			if (dependencyCreateResult.IsSuccess)
			{
				_workflowService.MarkApproved(workflowItem.WorkflowItemID, dependencyCreateResult.ID);
			}
		}

		public void Reject(WorkflowItem workflowItem)
		{
			throw new NotImplementedException();
		}
	}
}
