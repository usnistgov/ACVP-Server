using System.Collections.Generic;
using System.Linq;
using ACVPCore.Models.Parameters;
using ACVPCore.Providers;
using ACVPCore.Results;

namespace ACVPCore.Services
{
	public class OrganizationService : IOrganizationService
	{
		private readonly IOrganizationProvider _organizationProvider;
		private readonly IAddressService _addressService;

		public OrganizationService(IOrganizationProvider organizationProvider, IAddressService addressService)
		{
			_organizationProvider = organizationProvider;
			_addressService = addressService;
		}

		public DeleteResult Delete(long organizationID)
		{
			Result result;

			//Check to see if the dependency is used, in which case it can't be deleted
			if (OrganizationIsUsed(organizationID))
			{
				return new DeleteResult(DeleteResult.ErrorReason.IsInUse);
			}

			//Delete all organization addresses
			result = _addressService.DeleteAllForOrganization(organizationID);

			if (!result.IsSuccess)
			{
				return new DeleteResult(result);
			}

			//Delete all organization emails
			result = _organizationProvider.DeleteAllEmails(organizationID);

			if (!result.IsSuccess)
			{
				return new DeleteResult(result);
			}

			//Delete the Organization
			result = _organizationProvider.Delete(organizationID);

			return new DeleteResult(result);

		}

		public OrganizationResult Create(OrganizationCreateParameters parameters)
		{
			//Insert the organization record
			InsertResult organizationInsertResult = _organizationProvider.Insert(parameters.Name, parameters.Website, parameters.VoiceNumber, parameters.FaxNumber, parameters.ParentOrganizationID);

			if (!organizationInsertResult.IsSuccess)
			{
				return new OrganizationResult(organizationInsertResult.ErrorMessage);
			}

			//Insert the email addresses. Using a for loop instead of a foreach because the order of the addresses needs to be specified
			Result emailResult;
			for (int i = 0; i < parameters.EmailAddresses.Count; i++)
			{
				emailResult = _organizationProvider.InsertEmailAddress(organizationInsertResult.ID, parameters.EmailAddresses[i], i);
			}

			//Insert the addresses. Using a for loop because the org ID and the order index need to be included in the parameters
			InsertResult addressResult;
			for (int i = 0; i < parameters.Addresses.Count; i++)
			{
				parameters.Addresses[i].OrganizationID = organizationInsertResult.ID;
				parameters.Addresses[i].OrderIndex = i;

				addressResult = _addressService.Create(parameters.Addresses[i]);
			}

			return new OrganizationResult(organizationInsertResult.ID);
		}

		public OrganizationResult Update(OrganizationUpdateParameters parameters)
		{
			//Since updating the addresses of the organization may require deleting some, but addresses may be used not only by organizations but also by implementations, need to first check that we can safely do any address deletions before we update anything
			if (parameters.AddressesUpdated)
			{
				//Get the current address IDs
				IEnumerable<long> currentAddressIDs = _addressService.GetAllForOrganization(parameters.ID).Select(x => x.ID);

				//Extract the updated address IDs
				IEnumerable<long> updatedAddressesIDs = parameters.Addresses.Where(x => x is AddressUpdateParameters).Select(x => (AddressUpdateParameters)x).Select(x => x.ID);

				//Create a collection of the address IDs to be deleted
				IEnumerable<long> addressIDsToDelete = currentAddressIDs.Except(updatedAddressesIDs);

				//See if any of those to be deleted are in use, returning an error if there are
				if (addressIDsToDelete.Any(x => _addressService.AddressIsUsedOtherThanOrg(x))){
					return new OrganizationResult("Cannot update organization because an address that would be deleted is in use");
				}

				//Know it is safe to do any deletes needed by the address updates, and since we have all the things we need to do those updates just go ahead and do the address update now. This can be a combination of new addresses and updates, plus deletions if not included in this collection

				//Do the deletions
				foreach (long addressID in addressIDsToDelete)
				{
					_addressService.Delete(addressID);
				}

				//Add or update everything that was passed in
				Result addressResult;
				for (int i = 0; i < parameters.Addresses.Count; i++)
				{
					if (parameters.Addresses[i] is AddressCreateParameters)
					{
						addressResult = _addressService.Create((AddressCreateParameters)parameters.Addresses[i]);
					}
					else
					{
						addressResult = _addressService.Update((AddressUpdateParameters)parameters.Addresses[i]);
					}
				}
			}

			//Update the organization record if needed. Phone numbers go as a pair
			if (parameters.NameUpdated || parameters.WebsiteUpdated || parameters.ParentOrganizationIDUpdated || parameters.VoiceNumberUpdated || parameters.FaxNumberUpdated)
			{
				Result organizationUpdateResult = _organizationProvider.Update(parameters.ID, parameters.Name, parameters.Website, parameters.VoiceNumber, parameters.FaxNumber, parameters.ParentOrganizationID, parameters.NameUpdated, parameters.WebsiteUpdated, parameters.VoiceNumberUpdated, parameters.FaxNumberUpdated, parameters.ParentOrganizationIDUpdated);

				if (!organizationUpdateResult.IsSuccess)
				{
					return new OrganizationResult(organizationUpdateResult.ErrorMessage);
				}
			}

			//Do the email addresses if needed. This is a full replacement
			if (parameters.EmailAddressesUpdated)
			{
				//Delete all the existing ones
				_organizationProvider.DeleteAllEmails(parameters.ID);

				//Add everything passed in
				Result emailResult;
				for (int i = 0; i < parameters.EmailAddresses.Count; i++)
				{
					emailResult = _organizationProvider.InsertEmailAddress(parameters.ID, parameters.EmailAddresses[i], i);
				}
			}

			//Even though it is kind of stupid, return a result object that includes the URL, as I think that's what is expected to go into the workflow item
			return new OrganizationResult(parameters.ID);
		}

		public bool OrganizationIsUsed(long organizationID)
		{
			return _organizationProvider.OrganizationIsUsed(organizationID);
		}
	}
}
