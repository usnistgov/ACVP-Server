using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Results;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Shared.Results;


namespace NIST.CVP.Libraries.Internal.ACVPCore.Services
{
	public interface IOrganizationService
	{
		Organization Get(long organizationID);
		OrganizationResult Create(OrganizationCreateParameters parameters);
		OrganizationResult Update(OrganizationUpdateParameters parameters);
		DeleteResult Delete(long organizationID);
		bool OrganizationIsUsed(long organizationID);
		bool OrganizationExists(long organizationID);
		PagedEnumerable<OrganizationLite> Get(OrganizationListParameters param);
	}
}