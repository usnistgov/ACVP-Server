using ACVPCore.Models.Parameters;
using ACVPCore.Services;
using ACVPWorkflow.Exceptions;
using ACVPWorkflow.Models;

namespace ACVPWorkflow.WorkflowItemPayloadValidators
{
	public class DeleteOrganizationPayloadValidator : IWorkflowItemPayloadValidator
	{
		private readonly IOrganizationService _organizationService;

		public DeleteOrganizationPayloadValidator(IOrganizationService organizationService)
		{
			_organizationService = organizationService;
		}

		public bool Validate(IWorkflowItemPayload workflowItemPayload)
		{
			DeleteParameters parameters = ((DeletePayload)workflowItemPayload).ToDeleteParameters();

			//Verify that the organization exists
			if (!_organizationService.OrganizationExists(parameters.ID))
			{
				throw new ResourceDoesNotExistException($"Organization {parameters.ID} does not exist");
			}

			//Not really anything else to validate...

			return true;
		}
	}
}
