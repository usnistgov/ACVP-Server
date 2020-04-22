using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Exceptions;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.WorkflowItemPayloadValidators
{
	public class UpdateOrganizationPayloadValidator : IWorkflowItemPayloadValidator
	{
		private readonly IOrganizationService _organizationService;

		public UpdateOrganizationPayloadValidator(IOrganizationService organizationService)
		{
			_organizationService = organizationService;
		}

		public bool Validate(IWorkflowItemPayload workflowItemPayload)
		{
			OrganizationUpdateParameters parameters = ((OrganizationUpdatePayload)workflowItemPayload).ToOrganizationUpdateParameters();

			//Verify that the organization exists
			if (!_organizationService.OrganizationExists(parameters.ID))
			{
				throw new ResourceDoesNotExistException($"Organization {parameters.ID} does not exist");
			}

			//Verify that the parent org exists, if changed and didn't change to null
			if (parameters.ParentOrganizationIDUpdated && parameters.ParentOrganizationID != null && !_organizationService.OrganizationExists((long)parameters.ParentOrganizationID))
			{
				throw new ResourceDoesNotExistException($"Organization {parameters.ParentOrganizationID} does not exist");
			}

			return true;
		}
	}
}
