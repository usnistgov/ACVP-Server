using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using ACVPCore.Providers;
using ACVPCore.Results;
using System;
using System.Collections.Generic;

namespace ACVPCore.Services
{
	public class ImplementationService : IImplementationService
	{
		private readonly IImplementationProvider _implementationProvider;

		public ImplementationService(IImplementationProvider implementationProvider)
		{
			_implementationProvider = implementationProvider;
		}

		public Implementation Get(long implementationID)
		{
			return _implementationProvider.Get(implementationID);
		}
		public List<Implementation> ListImplementations(long pageSize, long pageNumber) {
			return _implementationProvider.GetImplementations(pageSize, pageNumber);
		}
		public DeleteResult Delete(long implementationID)
		{
			Result result;

			//Check to see if the implementation is used, in which case it can't be deleted
			if (ImplementationIsUsed(implementationID))
			{
				return new DeleteResult(DeleteResult.ErrorReason.IsInUse);
			}

			//Delete all contacts
			result = _implementationProvider.DeleteAllContacts(implementationID);

			if (!result.IsSuccess)
			{
				return new DeleteResult(result);
			}

			//Delete the Implementation
			result = _implementationProvider.Delete(implementationID);

			return new DeleteResult(result);

		}

		public ImplementationResult Create(ImplementationCreateParameters parameters)
		{
			//Insert the implementation record
			InsertResult implementationInsertResult = _implementationProvider.Insert(parameters.Name, parameters.Description, parameters.Type, parameters.Version, parameters.Website, parameters.OrganizationID, parameters.AddressID, parameters.IsITAR);

			if (!implementationInsertResult.IsSuccess)
			{
				return new ImplementationResult(implementationInsertResult.ErrorMessage);
			}

			//Insert the contact. Using a for loop instead of a foreach because the order needs to be specified
			Result contactResult;
			for (int i = 0; i < parameters.ContactIDs.Count; i++)
			{
				contactResult = _implementationProvider.InsertContact(implementationInsertResult.ID, parameters.ContactIDs[i], i);
			}

			return new ImplementationResult(implementationInsertResult.ID);
		}

		public ImplementationResult Update(ImplementationUpdateParameters parameters)
		{
			//Update the implementation record if needed.
			if (parameters.NameUpdated || parameters.DescriptionUpdated || parameters.TypeUpdated || parameters.VersionUpdated || parameters.WebsiteUpdated || parameters.OrganizationIDUpdated || parameters.AddressIDUpdated)
			{
				Result implementationUpdateResult = _implementationProvider.Update(parameters.ID, parameters.Name, parameters.Description, parameters.Type, parameters.Version, parameters.Website, parameters.OrganizationID, parameters.AddressID, parameters.NameUpdated, parameters.DescriptionUpdated, parameters.TypeUpdated, parameters.VersionUpdated, parameters.WebsiteUpdated, parameters.OrganizationIDUpdated, parameters.AddressIDUpdated);

				if (!implementationUpdateResult.IsSuccess)
				{
					return new ImplementationResult(implementationUpdateResult.ErrorMessage);
				}
			}

			//Do the contacts if needed. This is a full replacement
			if (parameters.ContactIDsUpdated)
			{
				//Delete all the existing ones
				_implementationProvider.DeleteAllContacts(parameters.ID);

				//Add everything passed in
				Result contactResult;
				for (int i = 0; i < parameters.ContactIDs.Count; i++)
				{
					contactResult = _implementationProvider.InsertContact(parameters.ID, parameters.ContactIDs[i], i);
				}
			}

			//Even though it is kind of stupid, return a result object that includes the URL, as I think that's what is expected to go into the workflow item
			return new ImplementationResult(parameters.ID);
		}

		public bool ImplementationIsUsed(long implementationID)
		{
			return _implementationProvider.ImplementationIsUsed(implementationID);
		}

		public bool ImplementationExists(long implementationID)
		{
			return _implementationProvider.ImplementationExists(implementationID);
		}
	}
}
