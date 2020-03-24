using ACVPCore.Models.Parameters;
using ACVPCore.Services;
using ACVPWorkflow.Exceptions;
using ACVPWorkflow.Models;

namespace ACVPWorkflow.WorkflowItemPayloadValidators
{
	public class CreatePersonPayloadValidator : IWorkflowItemPayloadValidator
	{
		private readonly IOrganizationService _organizationService;

		public CreatePersonPayloadValidator(IOrganizationService organizationService)
		{
			_organizationService = organizationService;
		}

		public bool Validate(IWorkflowItemPayload workflowItemPayload)
		{
			PersonCreateParameters parameters = ((PersonCreatePayload)workflowItemPayload).ToPersonCreateParameters();

			//Verify that the organization exists
			if (!_organizationService.OrganizationExists(parameters.OrganizationID))
			{
				throw new ResourceDoesNotExistException($"Organization {parameters.OrganizationID} does not exist");
			}

			return true;
		}
	}
}
