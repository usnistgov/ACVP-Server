using System.Linq;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Internal.ACVPWorkflow.Exceptions;
using NIST.CVP.Libraries.Internal.ACVPWorkflow.Models;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.WorkflowItemPayloadValidators
{
	public class CreateImplementationPayloadValidator : IWorkflowItemPayloadValidator
	{
		private readonly IOrganizationService _organizationService;
		private readonly IPersonService _personService;
		private readonly IAddressService _addressService;

		public CreateImplementationPayloadValidator(IOrganizationService organizationService, IPersonService personService, IAddressService addressService)
		{
			_organizationService = organizationService;
			_personService = personService;
			_addressService = addressService;
		}

		public bool Validate(IWorkflowItemPayload workflowItemPayload)
		{
			//Because the optional address thing is so goofy compared to other payloads, need to work with the raw payload instead of a more convenient parameters object
			ImplementationCreatePayload payload = (ImplementationCreatePayload)workflowItemPayload;

			//Verify that the organization exists
			long organizationID = payload.VendorObjectThatNeedsToGoAway.ID;
			if (!_organizationService.OrganizationExists(organizationID))
			{
				throw new ResourceDoesNotExistException($"Organization {organizationID} does not exist");
			}

			//Verify that the address exists, or if none specified, that the vendor has an address
			if (string.IsNullOrEmpty(payload.AddressURL))
			{
				//No address specified so check if the vendor has any
				if (!_addressService.GetAllForOrganization(payload.VendorObjectThatNeedsToGoAway.ID).Any())
				{
					throw new ResourceDoesNotExistException("Implementation create payload does not include address, and the referenced vendor has no addresses");
				}
			}
			else
			{
				//Specified an address, verify that it exists and it belongs to the same organization as specified here.
				long addressID = BasePayload.ParseIDFromURL(payload.AddressURL);

				Address address = _addressService.Get(addressID);

				if (address == null)
				{
					throw new ResourceDoesNotExistException($"Address {addressID} does not exist");
				}

				if (address.OrganizationID != organizationID)
				{
					throw new BusinessRuleException($"Address {addressID} is not an address of the referenced organization {organizationID}");
				}
			}

			//Verify that each of the contacts exists
			if (payload.ContactsObjectThatNeedsToGoAway != null)
			{
				foreach (long personID in payload.ContactsObjectThatNeedsToGoAway.Select(x => x.Person.ID))
				{
					if (!_personService.PersonExists(personID))
					{
						throw new ResourceDoesNotExistException($"Person {personID} does not exist");
					}
				}
			}

			return true;
		}
	}
}
