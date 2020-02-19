using System.Collections.Generic;
using ACVPCore.Models;
using ACVPCore.Results;

namespace ACVPCore.Providers
{
	public interface IAddressProvider
	{
		Result Delete(long addressID);
		Result DeleteAllForOrganization(long organizationID);
		InsertResult Insert(long organizationID, string street1, string street2, string street3, string locality, string region, string postalCode, string country, int orderIndex);
		Result Update(long addressID, string street1, string street2, string street3, string locality, string region, string postalCode, string country, int orderIndex, bool street1Updated, bool street2Updated, bool street3Updated, bool localityUpdated, bool regionUpdated, bool postalCodeUpdated, bool countryUpdated);
		Address Get(long addressID);
		List<Address> GetAllForOrganization(long organizationID);

		bool AddressIsUsedOtherThanOrg(long addressID);
		bool AddressExists(long addressID);
	}
}