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
	public class CreateImplementationProcessor : IWorkflowItemProcessor
	{
		private readonly IImplementationService _implementationService;

		public CreateImplementationProcessor(IImplementationService implementationService)
		{
			_implementationService = implementationService;
		}

		public long Approve(WorkflowItem workflowItem)
		{
			ImplementationCreateParameters parameters = ((ImplementationCreatePayload)workflowItem.Payload).ToImplementationCreateParameters();

			//Create it
			ImplementationResult implementationCreateResult = _implementationService.Create(parameters);

			if (!implementationCreateResult.IsSuccess)
			{
				throw new ResourceProcessorException($"Failed approval on {nameof(workflowItem.APIAction)} {workflowItem.APIAction}");
			}

			return implementationCreateResult.ID;
		}

		public void Reject(WorkflowItem workflowItem) { }
	}
}
