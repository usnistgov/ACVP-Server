using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Exceptions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.WorkflowItemPayloadValidators
{
	public class UpdateImplementationPayloadValidator : IWorkflowItemPayloadValidator
	{
		private readonly IImplementationService _implementationService;
		private readonly IOrganizationService _organizationService;
		private readonly IPersonService _personService;
		private readonly IAddressService _addressService;

		public UpdateImplementationPayloadValidator(IImplementationService implementationService, IOrganizationService organizationService, IPersonService personService, IAddressService addressService)
		{
			_implementationService = implementationService;
			_organizationService = organizationService;
			_personService = personService;
			_addressService = addressService;
		}

		public bool Validate(IWorkflowItemPayload workflowItemPayload)
		{
			ImplementationUpdateParameters parameters = ((ImplementationUpdatePayload)workflowItemPayload).ToImplementationUpdateParameters();

			//Verify that the implementation exists
			if (!_implementationService.ImplementationExists((long)parameters.ID))
			{
				throw new ResourceDoesNotExistException($"Implementation {parameters.ID} does not exist");
			}

			//Verify that the organization exists, if changed
			if (parameters.OrganizationIDUpdated && !_organizationService.OrganizationExists((long)parameters.OrganizationID))
			{
				throw new ResourceDoesNotExistException($"Organization {parameters.OrganizationID} does not exist");
			}

			//Verify that the address exists, if changed
			if (parameters.AddressIDUpdated && !_addressService.AddressExists((long)parameters.AddressID))
			{
				throw new ResourceDoesNotExistException($"Address {parameters.AddressID} does not exist");
			}

			//Verify that each of the contacts exists, if changed
			if (parameters.ContactIDsUpdated)
			{
				foreach (long personID in parameters.ContactIDs)
				{
					if (!_personService.PersonExists(personID))
					{
						throw new ResourceDoesNotExistException($"Person {parameters.OrganizationID} does not exist");
					}
				}
			}

			return true;
		}
	}
}
