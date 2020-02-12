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
	public class UpdateOrganizationProcessor : IWorkflowItemProcessor
	{
		private readonly IOrganizationService _organizationService;

		public UpdateOrganizationProcessor(IOrganizationService organizationService)
		{
			_organizationService = organizationService;
		}

		public long Approve(WorkflowItem workflowItem)
		{
			OrganizationUpdateParameters parameters = ((OrganizationUpdatePayload)workflowItem.Payload).ToOrganizationUpdateParameters();

			//Update it
			OrganizationResult organizationUpdateResult = _organizationService.Update(parameters);

			if (!organizationUpdateResult.IsSuccess)
			{
				throw new ResourceProcessorException($"Failed approval on {nameof(workflowItem.APIAction)} {workflowItem.APIAction}");
			}

			return organizationUpdateResult.ID;
		}

		public void Reject(WorkflowItem workflowItem) { }
	}
}
