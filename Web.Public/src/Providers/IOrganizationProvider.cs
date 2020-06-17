using System.Collections.Generic;
using Web.Public.Models;

namespace Web.Public.Providers
{
    public interface IOrganizationProvider
    {
        Organization Get(long id);
        bool Exists(long id);
        bool IsUsed(long id);
        (long TotalCount, List<Organization> Organizations) GetFilteredList(List<OrClause> orClauses, long offset, long limit);
        (long TotalCount, List<Person> Contacts) GetContactFilteredList(long organizationID, List<OrClause> orClauses, long offset, long limit);
    }
}