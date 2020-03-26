using System.Collections.Generic;
using Web.Public.Helpers;
using Web.Public.Models;

namespace Web.Public.Services
{
    public interface IOrganizationService
    {
        Organization Get(long organizationID);
        //PagedResponse<Organization> GetFilteredList(string filter, PagingOptions pagingOptions, string orDelimiter, string andDelimiter);
        (long TotalCount, List<Organization> Organizations) GetFilteredList(string filter, PagingOptions pagingOptions, string orDelimiter, string andDelimiter);
    }
}