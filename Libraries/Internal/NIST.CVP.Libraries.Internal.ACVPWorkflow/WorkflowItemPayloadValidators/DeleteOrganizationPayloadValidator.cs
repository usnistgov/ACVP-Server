using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Internal.ACVPWorkflow.Exceptions;
using NIST.CVP.Libraries.Internal.ACVPWorkflow.Models;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.WorkflowItemPayloadValidators
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
