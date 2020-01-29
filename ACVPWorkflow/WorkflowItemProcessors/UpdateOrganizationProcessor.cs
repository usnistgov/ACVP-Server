using System;
using System.Text.Json;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using ACVPWorkflow.Models;
using ACVPWorkflow.Services;

namespace ACVPWorkflow.WorkflowItemProcessors
{
	public class UpdateOrganizationProcessor : IWorkflowItemProcessor
	{
		private readonly IOrganizationService _organizationService;
		private readonly IWorkflowService _workflowService;

		public UpdateOrganizationProcessor(IOrganizationService organizationService, IWorkflowService workflowService)
		{
			_organizationService = organizationService;
			_workflowService = workflowService;
		}

		public void Approve(WorkflowItem workflowItem)
		{
			OrganizationUpdateParameters parameters = ((OrganizationUpdatePayload)workflowItem.Payload).ToOrganizationUpdateParameters();

			//Update it
			OrganizationResult organizationUpdateResult = _organizationService.Update(parameters);

			//Update the workflow item
			if (organizationUpdateResult.IsSuccess)
			{
				_workflowService.MarkApproved(workflowItem.WorkflowItemID, organizationUpdateResult.ID);
			}
		}

		public void Reject(WorkflowItem workflowItem)
		{
			throw new NotImplementedException();
		}
	}
}
