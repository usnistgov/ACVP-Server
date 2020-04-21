using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.Results;


namespace NIST.CVP.Libraries.Internal.ACVPCore.Services
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