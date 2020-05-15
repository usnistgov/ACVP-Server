using System.Collections.Generic;
using Web.Public.Models;

namespace Web.Public.Providers
{
    public interface IOrganizationProvider
    {
        Organization Get(long id);
        bool Exists(long id);
        bool IsUsed(long id);
        (long TotalCount, List<Organization> Organizations) GetFilteredList(string filter, long offset, long limit, string orDelimiter, string andDelimiter);
        (long TotalCount, List<Person> Contacts) GetContactFilteredList(long organizationId, string filter, long offset, long limit, string orDelimiter, string andDelimiter);
    }
}