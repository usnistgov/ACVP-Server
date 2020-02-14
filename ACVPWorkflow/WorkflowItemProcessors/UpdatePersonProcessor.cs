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
	public class UpdatePersonProcessor : IWorkflowItemProcessor
	{
		private readonly IPersonService _personService;

		public UpdatePersonProcessor(IPersonService personService)
		{
			_personService = personService;
		}

		public long Approve(WorkflowItem workflowItem)
		{
			PersonUpdateParameters parameters = ((PersonUpdatePayload)workflowItem.Payload).ToPersonUpdateParameters();

			//Update it
			PersonResult personUpdateResult = _personService.Update(parameters);

			if (!personUpdateResult.IsSuccess)
			{
				throw new ResourceProcessorException($"Failed approval on {nameof(workflowItem.APIAction)} {workflowItem.APIAction}");
			}

			return personUpdateResult.ID;
		}

		public void Reject(WorkflowItem workflowItem)
		{
			throw new NotImplementedException();
		}
	}
}
