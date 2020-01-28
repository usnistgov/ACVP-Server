using System.Collections.Generic;
using System.Linq;
using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using ACVPCore.Providers;
using ACVPCore.Results;

namespace ACVPCore.Services
{
	public class AddressService : IAddressService
	{
		private readonly IAddressProvider _addressProvider;

		public AddressService(IAddressProvider addressProvider)
		{
			_addressProvider = addressProvider;
		}

		public DeleteResult Delete(long addressID)
		{
			Result result;

			//Check to see if the address is used by anything other than an organization, in which case it can't be deleted
			if (AddressIsUsedOtherThanOrg(addressID))
			{
				return new DeleteResult(DeleteResult.ErrorReason.IsInUse);
			}

			//Delete the Address
			result = _addressProvider.Delete(addressID);

			return new DeleteResult(result);
		}

		public Result DeleteAllForOrganization(long organizationID)
		{
			return _addressProvider.DeleteAllForOrganization(organizationID);
		}

		public InsertResult Create(AddressCreateParameters parameters)
		{
			//Insert the address record
			return _addressProvider.Insert(parameters.OrganizationID, parameters.Street1, parameters.Street2, parameters.Street3, parameters.Locality, parameters.Region, parameters.PostalCode, parameters.Country, parameters.OrderIndex);
		}

		public Result Update(AddressUpdateParameters parameters)
		{
			return _addressProvider.Update(parameters.ID, parameters.Street1, parameters.Street2, parameters.Street3, parameters.Locality, parameters.Region, parameters.PostalCode, parameters.Country, parameters.OrderIndex, parameters.Street1Updated, parameters.Street2Updated, parameters.Street3Updated, parameters.LocalityUpdated, parameters.RegionUpdated, parameters.PostalCodeUpdated, parameters.CountryUpdated);
		}

		public List<Address> GetAllForOrganization(long organizationID)
		{
			return _addressProvider.GetAllForOrganization(organizationID);
		}

		public bool AddressIsUsedOtherThanOrg(long addressID)
		{
			return _addressProvider.AddressIsUsedOtherThanOrg(addressID);
		}
	}
}
