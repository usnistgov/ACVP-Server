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
	public class CreatePersonProcessor : IWorkflowItemProcessor
	{
		private readonly IPersonService _personService;

		public CreatePersonProcessor(IPersonService personService)
		{
			_personService = personService;
		}

		public long Approve(WorkflowItem workflowItem)
		{
			PersonCreateParameters parameters = ((PersonCreatePayload)workflowItem.Payload).ToPersonCreateParameters();

			//Create it
			PersonResult personCreateResult = _personService.Create(parameters);

			if (!personCreateResult.IsSuccess)
			{
				throw new ResourceProcessorException($"Failed approval on {nameof(workflowItem.APIAction)} {workflowItem.APIAction}");
			}

			return personCreateResult.ID;
		}

		public void Reject(WorkflowItem workflowItem) { }
	}
}
