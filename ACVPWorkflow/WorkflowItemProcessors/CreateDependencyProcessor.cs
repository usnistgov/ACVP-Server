using System;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using ACVPWorkflow.Models;
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
			DependencyCreateParameters parameters = ((DependencyCreatePayload)workflowItem.Payload).ToDependencyCreateParameters();

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
