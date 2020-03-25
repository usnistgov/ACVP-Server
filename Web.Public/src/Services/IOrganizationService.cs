using System.Collections.Generic;
using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using NIST.CVP.Results;
using Web.Public.Helpers;

namespace Web.Public.Services
{
    public interface IOrganizationService
    {
        Organization Get(long organizationID);
        OrganizationResult Create(OrganizationCreateParameters parameters);
        OrganizationResult Update(OrganizationUpdateParameters parameters);
        DeleteResult Delete(long organizationID);
        List<Organization> GetList(PagingOptions pagingOptions);
    }
}