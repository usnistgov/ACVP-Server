using System;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Results;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Exceptions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.WorkflowItemProcessors
{
	public class UpdatePersonProcessor : BaseWorkflowItemProcessor, IWorkflowItemProcessor
	{
		private readonly IPersonService _personService;
		private readonly IWorkflowItemPayloadValidatorFactory _workflowItemPayloadValidatorFactory;

		public UpdatePersonProcessor(IPersonService personService, IWorkflowItemPayloadValidatorFactory workflowItemPayloadValidatorFactory)
		{
			_personService = personService;
			_workflowItemPayloadValidatorFactory = workflowItemPayloadValidatorFactory;
		}

		public bool Validate(WorkflowItem workflowItem)
		{
			return IsPendingApproval(workflowItem) && _workflowItemPayloadValidatorFactory.GetWorkflowItemPayloadValidator(APIAction.UpdatePerson).Validate((PersonUpdatePayload)workflowItem.Payload);
		}

		public long Approve(WorkflowItem workflowItem)
		{
			//Validate this workflow item
			Validate(workflowItem);

			PersonUpdateParameters parameters = ((PersonUpdatePayload)workflowItem.Payload).ToPersonUpdateParameters();

			//Update it
			PersonResult result = _personService.Update(parameters);

			if (!result.IsSuccess)
			{
				throw new ResourceProcessorException($"Failed approval on {nameof(workflowItem.APIAction)} {workflowItem.APIAction}.{Environment.NewLine}Reason: {result.ErrorMessage}");
			}

			return result.ID;
		}

		public void Reject(WorkflowItem workflowItem)
		{
			throw new NotImplementedException();
		}
	}
}
