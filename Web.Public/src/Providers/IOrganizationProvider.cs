using System.Collections.Generic;
using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using Web.Public.Results;

namespace Web.Public.Providers
{
    public interface IOrganizationProvider
    {
        PagedEnumerable<OrganizationLite> GetList(OrganizationListParameters param);
        Organization Get(long id);
    }
}