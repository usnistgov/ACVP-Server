﻿using ACVPCore.Models.Parameters;
using ACVPCore.Services;
using ACVPWorkflow.Exceptions;
using ACVPWorkflow.Models;
using NIST.CVP.Results;

namespace ACVPWorkflow.WorkflowItemProcessors
{
	public class DeletePersonProcessor : BaseWorkflowItemProcessor, IWorkflowItemProcessor
	{
		private readonly IPersonService _personService;
		private readonly IWorkflowItemPayloadValidatorFactory _workflowItemPayloadValidatorFactory;

		public DeletePersonProcessor(IPersonService personService, IWorkflowItemPayloadValidatorFactory workflowItemPayloadValidatorFactory)
		{
			_personService = personService;
			_workflowItemPayloadValidatorFactory = workflowItemPayloadValidatorFactory;
		}

		public bool Validate(WorkflowItem workflowItem)
		{
			return IsPendingApproval(workflowItem) && _workflowItemPayloadValidatorFactory.GetWorkflowItemPayloadValidator(APIAction.DeletePerson).Validate((DeletePayload)workflowItem.Payload);
		}

		public long Approve(WorkflowItem workflowItem)
		{
			//Validate this workflow item
			Validate(workflowItem);

			//Deserialize the JSON
			DeleteParameters deleteParameters = ((DeletePayload)workflowItem.Payload).ToDeleteParameters();

			//Delete that person - will fail if person is in use
			DeleteResult deleteResult = _personService.Delete(deleteParameters.ID);

			if (deleteResult.IsInUse)
			{
				throw new ResourceInUseException($"The resource could not be deleted as other resources are referencing it. Workflow Info: {workflowItem.APIAction} {workflowItem.WorkflowItemID}");
			}
			
			if (!deleteResult.IsSuccess)
			{
				throw new ResourceProcessorException($"The resource could not be deleted. Workflow Info: {workflowItem.APIAction} {workflowItem.WorkflowItemID}");				
			}
			
			return deleteParameters.ID;
		}

		public void Reject(WorkflowItem workflowItem) { }
	}
}