using System;
using System.Text.Json;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using ACVPWorkflow.Models;
using ACVPWorkflow.Services;

namespace ACVPWorkflow.WorkflowItemProcessors
{
	public class CreateOrganizationProcessor : IWorkflowItemProcessor
	{
		private readonly IOrganizationService _organizationService;
		private readonly IWorkflowService _workflowService;

		public CreateOrganizationProcessor(IOrganizationService organizationService, IWorkflowService workflowService)
		{
			_organizationService = organizationService;
			_workflowService = workflowService;
		}

		public void Approve(WorkflowItem workflowItem)
		{
			OrganizationCreateParameters parameters = ((OrganizationCreatePayload)workflowItem.Payload).ToOrganizationCreateParameters();

			//Create it
			OrganizationResult organizationCreateResult = _organizationService.Create(parameters);

			//Update the workflow item
			if (organizationCreateResult.IsSuccess)
			{
				_workflowService.MarkApproved(workflowItem.WorkflowItemID, organizationCreateResult.ID);
			}
		}

		public void Reject(WorkflowItem workflowItem)
		{
			throw new NotImplementedException();
		}
	}
}
