using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;

namespace Web.Public.Services
{
    public interface IOrganizationService
    {
        Organization Get(long organizationID);
        OrganizationResult Create(OrganizationCreateParameters parameters);
        OrganizationResult Update(OrganizationUpdateParameters parameters);
        DeleteResult Delete(long organizationID);
        PagedEnumerable<OrganizationLite> GetList(OrganizationListParameters param);
    }
}