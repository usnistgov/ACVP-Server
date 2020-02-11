using System;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using ACVPWorkflow.Exceptions;
using ACVPWorkflow.Models;
using ACVPWorkflow.Services;

namespace ACVPWorkflow.WorkflowItemProcessors
{
	public class CreateDependencyProcessor : IWorkflowItemProcessor
	{
		private readonly IDependencyService _dependencyService;

		public CreateDependencyProcessor(IDependencyService dependencyService)
		{
			_dependencyService = dependencyService;
		}

		public long Approve(WorkflowItem workflowItem)
		{
			DependencyCreateParameters parameters = ((DependencyCreatePayload)workflowItem.Payload).ToDependencyCreateParameters();

			//Create it
			DependencyResult dependencyCreateResult = _dependencyService.Create(parameters);

			if (!dependencyCreateResult.IsSuccess)
			{
				throw new ResourceProcessorException($"Failed approval on {nameof(workflowItem.APIAction)} {workflowItem.APIAction}");
			}
			
			return dependencyCreateResult.ID;
		}

		public void Reject(WorkflowItem workflowItem) { }
	}
}
