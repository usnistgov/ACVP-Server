using System;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Results;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Exceptions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.WorkflowItemProcessors
{
	public class CreateOrganizationProcessor : BaseWorkflowItemProcessor, IWorkflowItemProcessor
	{
		private readonly IOrganizationService _organizationService;
		private readonly IWorkflowItemPayloadValidatorFactory _workflowItemPayloadValidatorFactory;

		public CreateOrganizationProcessor(IOrganizationService organizationService, IWorkflowItemPayloadValidatorFactory workflowItemPayloadValidatorFactory)
		{
			_organizationService = organizationService;
			_workflowItemPayloadValidatorFactory = workflowItemPayloadValidatorFactory;
		}

		public bool Validate(WorkflowItem workflowItem)
		{
			return IsPendingApproval(workflowItem) && _workflowItemPayloadValidatorFactory.GetWorkflowItemPayloadValidator(APIAction.CreateVendor).Validate((OrganizationCreatePayload)workflowItem.Payload);
		}

		public long Approve(WorkflowItem workflowItem)
		{
			//Validate this workflow item
			Validate(workflowItem);

			OrganizationCreateParameters parameters = ((OrganizationCreatePayload)workflowItem.Payload).ToOrganizationCreateParameters();

			//Create it
			OrganizationResult result = _organizationService.Create(parameters);

			if (!result.IsSuccess)
			{
				throw new ResourceProcessorException($"Failed approval on {nameof(workflowItem.APIAction)} {workflowItem.APIAction}.{Environment.NewLine}Reason: {result.ErrorMessage}");
			}

			return result.ID;
		}

		public void Reject(WorkflowItem workflowItem) { }
	}
}
