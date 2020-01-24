using System;
using System.Text.Json;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using ACVPWorkflow.Models;
using ACVPWorkflow.Services;

namespace ACVPWorkflow.WorkflowItemProcessors
{
	public class UpdatePersonProcessor : IWorkflowItemProcessor
	{
		private readonly IPersonService _personService;
		private readonly IWorkflowService _workflowService;

		public UpdatePersonProcessor(IPersonService personService, IWorkflowService workflowService)
		{
			_personService = personService;
			_workflowService = workflowService;
		}

		public void Approve(WorkflowItem workflowItem)
		{
			PersonUpdateParameters parameters = JsonSerializer.Deserialize<PersonUpdatePayload>(workflowItem.JSON).ToPersonUpdateParameters();

			//Update it
			PersonResult personUpdateResult = _personService.Update(parameters);

			//Update the workflow item
			if (personUpdateResult.IsSuccess)
			{
				_workflowService.MarkApproved(workflowItem.WorkflowItemID, personUpdateResult.ID);
			}
		}

		public void Reject(WorkflowItem workflowItem)
		{
			throw new NotImplementedException();
		}
	}
}
