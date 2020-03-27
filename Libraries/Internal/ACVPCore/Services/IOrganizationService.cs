using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using NIST.CVP.Enumerables;
using NIST.CVP.Results;


namespace ACVPCore.Services
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