using System.Collections.Generic;
using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;

namespace ACVPCore.Services
{
	public interface IAddressService
	{
		InsertResult Create(AddressCreateParameters parameters);
		Result DeleteAllForOrganization(long organizationID);
		DeleteResult Delete(long addressID);
		Result Update(AddressUpdateParameters parameters);
		List<Address> GetAllForOrganization(long organizationID);
		bool AddressIsUsedOtherThanOrg(long addressID);
		bool AddressExists(long addressID);
		Address Get(long addressID);
	}
}