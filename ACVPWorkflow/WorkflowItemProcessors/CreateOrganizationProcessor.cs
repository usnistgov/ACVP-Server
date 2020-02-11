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
	public class CreateOrganizationProcessor : IWorkflowItemProcessor
	{
		private readonly IOrganizationService _organizationService;

		public CreateOrganizationProcessor(IOrganizationService organizationService)
		{
			_organizationService = organizationService;
		}

		public long Approve(WorkflowItem workflowItem)
		{
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
