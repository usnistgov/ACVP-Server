using System.Collections.Generic;
using ACVPCore.Models;
using Web.Public.Helpers;

namespace Web.Public.Providers
{
    public interface IOrganizationProvider
    {
        List<Organization> GetList(PagingOptions pagingOptions);
        Organization Get(long id);
    }
}