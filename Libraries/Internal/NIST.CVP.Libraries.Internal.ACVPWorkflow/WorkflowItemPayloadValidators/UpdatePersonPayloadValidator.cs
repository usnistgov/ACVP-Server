using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Internal.ACVPWorkflow.Exceptions;
using NIST.CVP.Libraries.Internal.ACVPWorkflow.Models;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.WorkflowItemPayloadValidators
{
	public class UpdatePersonPayloadValidator : IWorkflowItemPayloadValidator
	{
		private readonly IOrganizationService _organizationService;
		private readonly IPersonService _personService;

		public UpdatePersonPayloadValidator(IPersonService personService, IOrganizationService organizationService)
		{
			_organizationService = organizationService;
			_personService = personService;
		}

		public bool Validate(IWorkflowItemPayload workflowItemPayload)
		{
			PersonUpdateParameters parameters = ((PersonUpdatePayload)workflowItemPayload).ToPersonUpdateParameters();

			//Verify that the person exists
			if (!_personService.PersonExists(parameters.ID))
			{
				throw new ResourceDoesNotExistException($"Person {parameters.ID} does not exist");
			}

			//Verify that the Organization exists, if it has been changed
			if (parameters.OrganizationIDUpdated && !_organizationService.OrganizationExists((long)parameters.OrganizationID))
			{
				throw new ResourceDoesNotExistException($"Organization {parameters.OrganizationID} does not exist");
			}

			return true;
		}
	}
}
