using ACVPCore.Models.Parameters;
using ACVPCore.Results;

namespace ACVPCore.Services
{
	public interface IOrganizationService
	{
		OrganizationResult Create(OrganizationCreateParameters parameters);
		OrganizationResult Update(OrganizationUpdateParameters parameters);
		DeleteResult Delete(long organizationID);
		bool OrganizationIsUsed(long organizationID);
	}
}