using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using ACVPWorkflow.Exceptions;
using ACVPWorkflow.Models;

namespace ACVPWorkflow.WorkflowItemProcessors
{
	public class CreatePersonProcessor : BaseWorkflowItemProcessor, IWorkflowItemProcessor
	{
		private readonly IPersonService _personService;
		private readonly IWorkflowItemPayloadValidatorFactory _workflowItemPayloadValidatorFactory;

		public CreatePersonProcessor(IPersonService personService, IWorkflowItemPayloadValidatorFactory workflowItemPayloadValidatorFactory)
		{
			_personService = personService;
			_workflowItemPayloadValidatorFactory = workflowItemPayloadValidatorFactory;
		}

		public bool Validate(WorkflowItem workflowItem)
		{
			return IsPendingApproval(workflowItem) && _workflowItemPayloadValidatorFactory.GetWorkflowItemPayloadValidator(APIAction.CreatePerson).Validate((PersonCreatePayload)workflowItem.Payload);
		}

		public long Approve(WorkflowItem workflowItem)
		{
			//Validate this workflow item
			Validate(workflowItem);

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
