using NIST.CVP.Libraries.Internal.ACVPCore.Models.Parameters;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Internal.ACVPWorkflow.Exceptions;
using NIST.CVP.Libraries.Internal.ACVPWorkflow.Models;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.WorkflowItemPayloadValidators
{
	public class CreateOrganizationPayloadValidator : IWorkflowItemPayloadValidator
	{
		private readonly IOrganizationService _organizationService;

		public CreateOrganizationPayloadValidator(IOrganizationService organizationService)
		{
			_organizationService = organizationService;
		}

		public bool Validate(IWorkflowItemPayload workflowItemPayload)
		{
			OrganizationCreateParameters parameters = ((OrganizationCreatePayload)workflowItemPayload).ToOrganizationCreateParameters();

			//Verify that the parent organization, if one is specified, exists
			if (parameters.ParentOrganizationID != null && !_organizationService.OrganizationExists((long)parameters.ParentOrganizationID))
			{
				throw new ResourceDoesNotExistException($"Organization {parameters.ParentOrganizationID} does not exist");
			}

			return true;
		}
	}
}
