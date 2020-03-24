using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using ACVPWorkflow.Exceptions;
using ACVPWorkflow.Models;

namespace ACVPWorkflow.WorkflowItemProcessors
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
			OrganizationResult organizationCreateResult = _organizationService.Create(parameters);

			if (!organizationCreateResult.IsSuccess)
			{
				throw new ResourceProcessorException($"Failed approval on {nameof(workflowItem.APIAction)} {workflowItem.APIAction}");
			}

			return organizationCreateResult.ID;
		}

		public void Reject(WorkflowItem workflowItem) { }
	}
}
