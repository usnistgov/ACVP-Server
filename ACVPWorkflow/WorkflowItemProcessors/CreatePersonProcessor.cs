using System;
using System.Text.Json;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using ACVPWorkflow.Models;
using ACVPWorkflow.Services;

namespace ACVPWorkflow.WorkflowItemProcessors
{
	public class CreatePersonProcessor : IWorkflowItemProcessor
	{
		private readonly IPersonService _personService;
		private readonly IWorkflowService _workflowService;

		public CreatePersonProcessor(IPersonService personService, IWorkflowService workflowService)
		{
			_personService = personService;
			_workflowService = workflowService;
		}

		public void Approve(WorkflowItem workflowItem)
		{
			PersonCreateParameters parameters = ((PersonCreatePayload)workflowItem.Payload).ToPersonCreateParameters();

			//Create it
			PersonResult personCreateResult = _personService.Create(parameters);

			//Update the workflow item
			if (personCreateResult.IsSuccess)
			{
				_workflowService.MarkApproved(workflowItem.WorkflowItemID, personCreateResult.ID);
			}
		}

		public void Reject(WorkflowItem workflowItem)
		{
			throw new NotImplementedException();
		}
	}
}
