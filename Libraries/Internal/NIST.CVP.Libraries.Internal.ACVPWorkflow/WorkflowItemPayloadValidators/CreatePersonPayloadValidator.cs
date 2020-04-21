using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Internal.ACVPWorkflow.Exceptions;
using NIST.CVP.Libraries.Internal.ACVPWorkflow.Models;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.WorkflowItemPayloadValidators
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
