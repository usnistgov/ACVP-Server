using System;
using System.Text.Json;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using ACVPWorkflow.Exceptions;
using ACVPWorkflow.Models;
using ACVPWorkflow.Services;

namespace ACVPWorkflow.WorkflowItemProcessors
{
	public class UpdateDependencyProcessor : IWorkflowItemProcessor
	{
		private readonly IDependencyService _dependencyService;

		public UpdateDependencyProcessor(IDependencyService dependencyService)
		{
			_dependencyService = dependencyService;
		}

		public long Approve(WorkflowItem workflowItem)
		{
			DependencyUpdateParameters parameters = ((DependencyUpdatePayload)workflowItem.Payload).ToDependencyUpdateParameters();

			//Update it
			DependencyResult dependencyUpdateResult = _dependencyService.Update(parameters);

			if (!dependencyUpdateResult.IsSuccess)
			{
				throw new ResourceProcessorException($"Failed approval on {nameof(workflowItem.APIAction)} {workflowItem.APIAction}");
			}

			return dependencyUpdateResult.ID;
		}

		public void Reject(WorkflowItem workflowItem) { }
	}
}
