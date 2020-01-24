using System;
using System.Text.Json;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using ACVPWorkflow.Models;
using ACVPWorkflow.Services;

namespace ACVPWorkflow.WorkflowItemProcessors
{
	public class UpdateDependencyProcessor : IWorkflowItemProcessor
	{
		private readonly IDependencyService _dependencyService;
		private readonly IWorkflowService _workflowService;

		public UpdateDependencyProcessor(IDependencyService dependencyService, IWorkflowService workflowService)
		{
			_dependencyService = dependencyService;
			_workflowService = workflowService;
		}

		public void Approve(WorkflowItem workflowItem)
		{
			DependencyUpdateParameters parameters = JsonSerializer.Deserialize<DependencyUpdatePayload>(workflowItem.JSON).ToDependencyUpdateParameters();

			//Update it
			DependencyResult dependencyUpdateResult = _dependencyService.Update(parameters);

			//Update the workflow item
			if (dependencyUpdateResult.IsSuccess)
			{
				_workflowService.MarkApproved(workflowItem.WorkflowItemID, dependencyUpdateResult.ID);
			}
		}

		public void Reject(WorkflowItem workflowItem)
		{
			throw new NotImplementedException();
		}
	}
}
