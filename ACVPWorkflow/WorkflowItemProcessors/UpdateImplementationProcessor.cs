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
	public class UpdateImplementationProcessor : IWorkflowItemProcessor
	{
		private readonly IImplementationService _implementationService;

		public UpdateImplementationProcessor(IImplementationService implementationService)
		{
			_implementationService = implementationService;
		}

		public long Approve(WorkflowItem workflowItem)
		{
			ImplementationUpdateParameters parameters = ((ImplementationUpdatePayload)workflowItem.Payload).ToImplementationUpdateParameters();

			//Update it
			ImplementationResult implementationUpdateResult = _implementationService.Update(parameters);

			if (!implementationUpdateResult.IsSuccess)
			{
				throw new ResourceProcessorException($"Failed approval on {nameof(workflowItem.APIAction)} {workflowItem.APIAction}");
			}

			return implementationUpdateResult.ID;
		}

		public void Reject(WorkflowItem workflowItem) { }
	}
}
