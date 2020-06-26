using System.Collections.Generic;
using Web.Public.Models;

namespace Web.Public.Services
{
    public interface IOrganizationService
    {
        Organization Get(long organizationID);

        bool Exists(long organizationID);

        bool IsUsed(long organizationID);
        (long TotalCount, List<Organization> Organizations) GetFilteredList(List<OrClause> orClauses, PagingOptions pagingOptions);
        (long TotalCount, List<Person> Contacts) GetContactFilteredList(long organizationId, List<OrClause> orClauses, PagingOptions pagingOptions);
    }
}